using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
//using LOR4;
using FileHelper;

namespace xLights22
{

	// Declared in RGBEffects
	//public enum xModelBaseTypes { Unknown, Model, RGBModel, PixelModel, ModelGroup }
	public enum xModelSubType { Unknown, Single_Line, Custom, Candy_Cane, Arch, Horz_Matrix, Image, Poly_Line, Tree, Circle, Star }
	public enum xSections { Header, Models, ViewObjects, Effects, Views, Palettes, ModelGroups, LayoutGroups, Perspectives, Settings, Viewpoints }

	// Note: The name of this is misleading but is used anyway to correspond to xLight's "RGBEffects.xml" file.
	// It contains (amongst other things) all the models in a layout,
	// But does NOT include the effects.
	public class xRGBEffects
	{

		public List<xModel> Models = new List<xModel>();
		public List<xRGBModel> RGBModels = new List<xRGBModel>();
		public List<xPixelModel> PixelModels = new List<xPixelModel>();
		public List<xModelGroup> ModelGroups = new List<xModelGroup>();
		private string RGBeffectsFile = "";
		private bool isWiz = Fyle.isWiz;
		public static bool SortByName = false;

		public xRGBEffects()
		{
			string theFile = xAdmin.ShowDirectory;
			if (Directory.Exists(theFile))
			{
				theFile += "\\xlights_rgbeffects.xml";
				if (File.Exists(theFile))
				{
					LoadRGBEffects(theFile);
				}
			}
		}

		public xRGBEffects(string rgbeffectsFile)
		{
			LoadRGBEffects(rgbeffectsFile);
		}

		public int LoadRGBEffects(string rgbeffectsFile)
		{
			int errs = 0;
			int lineCount = 3;
			string lineIn = "";
			xSections section = xSections.Header;

			RGBeffectsFile = rgbeffectsFile;
			try
			{
				StreamReader reader = new StreamReader(rgbeffectsFile);
				lineIn = reader.ReadLine(); // xml version
				lineIn = reader.ReadLine(); // <xrgb>
																		//lineIn = reader.ReadLine(); // <models>


				while (!reader.EndOfStream)
				{
					try
					{
						lineIn = reader.ReadLine();
						if (lineIn.IndexOf("<models") > 0)
						{
							section = xSections.Models;
							lineIn = reader.ReadLine();
						}
						else
						{
							if (lineIn.IndexOf("<modelGroups") > 0)
							{
								section = xSections.ModelGroups;
								lineIn = reader.ReadLine();
							}
							else
							{
								if (lineIn.IndexOf("<view_objects") > 0)
								{
									section = xSections.ViewObjects;
									lineIn = reader.ReadLine();
								}
								else
								{
									if (lineIn.IndexOf("<effects") > 0)
									{
										section = xSections.Effects;
										lineIn = reader.ReadLine();
									}
									else
									{
										if (lineIn.IndexOf("<views") > 0)
										{
											section = xSections.Views;
											lineIn = reader.ReadLine();
										}
										else
										{
											if (lineIn.IndexOf("<palettes") > 0)
											{
												section = xSections.ModelGroups;
												lineIn = reader.ReadLine();
											}
											else
											{
												if (lineIn.IndexOf("<layoutGroups") > 0)
												{
													section = xSections.ModelGroups;
													lineIn = reader.ReadLine();
												}
												else
												{
													if (lineIn.IndexOf("<perspectives") > 0)
													{
														section = xSections.ModelGroups;
														lineIn = reader.ReadLine();
													}
													else
													{
														if (lineIn.IndexOf("<settings") > 0)
														{
															section = xSections.ModelGroups;
															lineIn = reader.ReadLine();
														}
														else
														{
															if (lineIn.IndexOf("<colors") > 0)
															{
																section = xSections.ModelGroups;
																lineIn = reader.ReadLine();
															}
															else
															{
																if (lineIn.IndexOf("<Viewpoints") > 0)
																{
																	section = xSections.ModelGroups;
																	lineIn = reader.ReadLine();
																} // End start ov Viewpoints
															} // End start of Colors
														} // End of Settings
													} // End start of Perspective
												} // End start of Layout Groups
											} // End start of Palettes
										} // End start of View
									} // End start of Effects
								} // End start of View Objects
							} // End start of ModelGroups
						} // End start of Models


						if (section == xSections.Models)
						{

							string modelType = xAdmin.getKeyWord(lineIn, "DisplayAs");
							string modelName = xAdmin.getKeyWord(lineIn, "name");

							if (modelName.IndexOf("Bushes") >= 0)
							{
								string ItsABush = modelName;
							}

							//int xAddress = LOR4Admin.getKeyValue(lineIn, "StartChannel");
							string startAddress = xAdmin.getKeyWord(lineIn, "StartChannel");
							string mtl = modelType.ToLower();
							string stringType = xAdmin.getKeyWord(lineIn, "StringType");
							string stl = stringType.ToLower();
							xModelSubType xType = xModelSubType.Unknown;
							xPixelModel pixels = new xPixelModel("");
							xModel model = new xModel("");
							xRGBModel rgbModel = new xRGBModel("");

							ixMember member = null;

							int xAddress = -1;
							if (startAddress.Length > 0)
							{
								//TODO figure out how to extract an address
								//xAddress = xAdmin.GetAddress(startAddress, xModel.AllModels);
							}


							if (mtl.Length > 3)
							{
								string mt4 = mtl.Substring(0, 4);
								if (mt4.CompareTo("tree") == 0)
								{
									xType = xModelSubType.Tree;
									//model = new xModel(modelName, xType, xAddress);
									//xModels.Add(model);
									//member = model;
									//TODO Get Angle
								}
							}
							if (xType == xModelSubType.Unknown)
							{
								if (mtl.Length > 0)
								{
									switch (mtl)
									{
										case "single line":
											xType = xModelSubType.Single_Line;
											break;
										case "custom":
											xType = xModelSubType.Custom;
											break;
										case "candy canes":
											xType = xModelSubType.Candy_Cane;
											break;
										case "arches":
											xType = xModelSubType.Arch;
											break;
										case "horiz matrix":
											xType = xModelSubType.Horz_Matrix;
											break;
										case "image":
											xType = xModelSubType.Image;
											break;
										case "poly line":
											xType = xModelSubType.Poly_Line;
											break;
										case "circle":
											xType = xModelSubType.Circle;
											break;
										case "star":
											xType = xModelSubType.Star;
											break;
										default:
											if (isWiz)
											{
												string foo1 = modelType;
												System.Diagnostics.Debugger.Break();
											}
											xType = xModelSubType.Unknown;
											break;
									}
								}
							}

							if (stringType.Length > 0)
							{
								if (stringType.IndexOf("Single Color") == 0)
								{
									model = new xModel(modelName, xType, xAddress);
									Models.Add(model);
									member = model;
									//TODO Get Color
								}
								else
								{
									if (stringType.IndexOf("RGB Nodes") == 0)
									{
										pixels = new xPixelModel(modelName, xType, xAddress);
										PixelModels.Add(pixels);
										member = pixels;
									}
									else
									{
										if (stringType.IndexOf("Strobes") == 0)
										{
											model = new xModel(modelName, xType, xAddress);
											Models.Add(model);
											member = model;
											//TODO Get Color
										}
										else
										{
											if (stringType.IndexOf("3 Channel RGB") == 0)
											{
												rgbModel = new xRGBModel(modelName, xType, xAddress);
												RGBModels.Add(rgbModel);
												member = model;
												//TODO Get Color
											}
											else
											{
												if (isWiz)
												{
													string foo2 = stringType;
													System.Diagnostics.Debugger.Break();
												}
											} // End 3-LORChannel4 RGB
										} // End Strobes
									} // End RGB Nodes
								} // End Single Color
							} // End StringType Length
						} // End ParseModels
						int qqm = Models.Count;

						if (section == xSections.ModelGroups)
						{
							string modelName = xAdmin.getKeyWord(lineIn, "name");
							if (modelName.Length > 0)
							{
								string groupMembers = xAdmin.getKeyWord(lineIn, "models");
								xModelGroup group = new xModelGroup(modelName);
								string[] membrz = groupMembers.Split(',');
								foreach (string membr in membrz)
								{
									bool found = false;
									for (int m = 0; m < Models.Count; m++)
									{
										xModel model = Models[m];
										if (membr.CompareTo(model.Name) == 0)
										{
											group.Members.Add(model);
											found = true;
											m = Models.Count; // Exit loop
										}
									}
									if (!found)
									{
										for (int g = 0; g < ModelGroups.Count; g++)
										{
											xModelGroup grp2 = ModelGroups[g];
											if (membr.CompareTo(grp2.Name) == 0)
											{
												group.Members.Add(grp2);
												found = true;
												g = ModelGroups.Count; // Exit loop
											}
										} // end loop thru other groups
									} // End not found in other groups
								} // End loop thru members (from split[])
								ModelGroups.Add(group);
							} // End Name.Length
						} // End section ModelGroups
						int qqg = ModelGroups.Count;
					} // End Try
					catch (Exception ex2)
					{
						string msg = "Error while reading rgbeffects line " + lineCount.ToString() + "\r\n";
						msg += lineIn + "\r\n\r\n";
						msg += ex2.ToString();
						if (isWiz)
						{
							DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						errs++;
					} // End Catch
					lineCount++;
				} // End While Not End-of-Stream
				reader.Close();
				int qqqqm = Models.Count;
				int qqqqg = ModelGroups.Count;

				// Try go get any missing start addresses
				for (int m = 0; m < Models.Count; m++)
				{
					xModel model = Models[m];
					if (model.xLightsAddress < 1)
					{
						if (model.StartChannel.Length > 5)
						{
							//TODO: Figure out how to extract an address
							//int a = xAdmin.GetAddress(model.StartChannel, xModel.AllModels);
							int a = 1;
							model.xLightsAddress = a;
						}
					}
				}

			} // End Try
			catch (Exception ex1)
			{
				string msg = "Error while opening rgbeffects file\r\n\r\n";
				msg += ex1.ToString();

				if (isWiz)
				{
					DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				errs++;
			} // End Catch

			return errs;
		} // End Load RGBeffects



	} // End public class RGBEffects
} // End Namespace
