using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FileHelper;

namespace LOR4
{
	public partial class LOR4Sequence : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>, IDisposable
	{
		#region XML Tag Constants
		public const string TABLEsequence = "sequence";
		public const string TABLEchannelConfig = "channelConfig";
		public const string TABLErgbChannel = "rgbChannel";
		public const string TABLEchannelGroup = "channelGroup";
		public const string TABLEchannelGroupList = "channelGroupList";
		public const string FIELDchannelGroup = "channelGroup";
		public const string TABLEcellDemarcation = "cellDemarcation";
		public const string TABLEchannelsClipboard = "channelsClipboard";
		public const string TABLEeffect = "effect";
		public const string TABLEtimingGrid = "timingGrid";
		public const string TABLEtrack = "track";
		public const string TABLEcosmicDevice = "cosmicColorDevice";
		public const string STARTchannels = "<channels>";
		public const string STARTtracks = "<Tracks>";
		public const string STARTgrids = "<TimingGrids>";
		public const string TABLEloopLevels = "loopLevels";
		public const string TABLEloopLevel = "loopLevel";
		public const string STARTloops = "<loopLevels>";
		public const string TABLEanimation = "animation";

		private const string STARTsequence = LOR4Admin.STFLD + TABLEsequence + LOR4SeqInfo.FIELDsaveFileVersion;
		private const string STARTconfig = LOR4Admin.STFLD + TABLEchannelConfig + LOR4SeqInfo.FIELDchannelConfigFileVersion;
		private const string STARTeffect = LOR4Admin.STFLD + TABLEeffect + LOR4Admin.FIELDtype;
		public const string STARTchannel = LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.FIELDname;
		private const string STARTcosmic = LOR4Admin.STFLD + TABLEcosmicDevice + LOR4Admin.SPC;
		public const string STARTrgbChannel = LOR4Admin.STFLD + TABLErgbChannel + LOR4Admin.SPC;
		public const string STARTchannelGroup = LOR4Admin.STFLD + TABLEchannelGroupList + LOR4Admin.SPC;
		private const string STARTtrack = LOR4Admin.STFLD + TABLEtrack + LOR4Admin.SPC;
		private const string STARTtrackItem = LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.FIELDsavedIndex + LOR4Admin.FIELDEQ;
		private const string STARTtimingGrid = LOR4Admin.STFLD + TABLEtimingGrid + LOR4Admin.SPC;
		private const string STARTtiming = LOR4Admin.STFLD + LOR4Timings.TABLEtiming + LOR4Admin.SPC;
		private const string STARTgridItem = LOR4Timings.TABLEtiming + LOR4Admin.FIELDcentisecond;
		private const string STARTloopLevel = LOR4Admin.STFLD + TABLEloopLevel + LOR4Admin.FINFLD;
		private const string STARTloop = LOR4Admin.STFLD + LOR4Loop.FIELDloop + LOR4Admin.SPC;
		private const string STARTaniRow = LOR4Admin.STFLD + LOR4AnimationRow.FIELDrow + LOR4Admin.SPC + LOR4AnimationRow.FIELDindex;
		private const string STARTaniCol = LOR4Admin.STFLD + LOR4AnimationColumn.FIELDcolumnIndex;


		#endregion

		public const int ERROR_Undefined = LOR4Admin.UNDEFINED;
		public const int ERROR_NONE = 0;
		public const int ERROR_CantOpen = -101;
		public const int ERROR_NotXML = -102;
		public const int ERROR_NotSequence = -103;
		public const int ERROR_EncryptedDemo = -104;
		public const int ERROR_Compressed = -105;
		public const int ERROR_NoChannels = -106;
		public const int ERROR_PrematureEnd = -107;
		public const int ERROR_UnsupportedVersion = -114;
		public const int ERROR_UnexpectedData = -50;


		// members should only contain tracks, which are the only DIRECT descendants, in
		// keeping with ChannelGroups, Tracks, and Cosmic Parent items
		public LOR4Membership Members = null;
		// AllMembers contain ALL the regular members including Channels and RGBChannels and
		// ChannelGroups which are not DIRECT descendants of the sequence (only Tracks are)
		public LOR4Membership AllMembers = null;

		public List<LOR4Channel> Channels = new List<LOR4Channel>();
		public List<LOR4RGBChannel> RGBchannels = new List<LOR4RGBChannel>();
		public List<LOR4ChannelGroup> ChannelGroups = new List<LOR4ChannelGroup>();
		public List<LOR4Cosmic> CosmicDevices = new List<LOR4Cosmic>();
		public List<LOR4Timings> TimingGrids = new List<LOR4Timings>();
		public List<LOR4Track> Tracks = new List<LOR4Track>();
		public LOR4Animation animation = null;
		public LOR4SeqInfo info = null;
		public int videoUsage = 0;
		public int errorStatus = 0;
		public int lineCount = 0;
		//public int HighestSaveID = LOR4Admin.UNDEFINED;
		public int HighestAltSaveID = LOR4Admin.UNDEFINED;
		//public int HighestTrackIndex = LOR4Admin.UNDEFINED;
		public int HighestAltTrackNumber = LOR4Admin.UNDEFINED;

		// For now at least, this will remain false, therefore ALL timing grids will ALWAYS get written
		private bool WriteSelectedGridsOnly = false;

		//private string myName = "$_UNNAMED_$";
		protected StreamWriter writer;
		protected StreamReader reader;
		protected string lineOut = ""; // line to be Written out, gets modified if necessary

		//private int curSavedIndex = 0;
		private string tempFileName;
		private static string tempWorkPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\UtilORama\\";
		private string newFilename;
		private struct Match
		{
			public string Name;
			public int savedIdx;
			public LOR4MemberType MemberType;
			public int itemIdx;
		}

		public static bool debugMode = Fyle.DebugMode;

		#region Constructors
		public LOR4Sequence()
		{
			//! DEFAULT CONSTRUCTOR
			myName = "(New Sequence)";
			this.Members = new LOR4Membership(this);
			this.AllMembers = new LOR4Membership(this);
			//this.Members.SetParent(this);
			this.info = new LOR4SeqInfo(this);
			this.animation = new LOR4Animation(this);
			this.info.sequenceType = LOR4SequenceType.Animated;
			//Members.SetParentSequence(this);
		}

		public LOR4Sequence(string lineIn)
		{
			myName = lineIn;
			this.Members = new LOR4Membership(this);
			this.AllMembers = new LOR4Membership(this);
			//this.Members.SetParent(this);
			this.info = new LOR4SeqInfo(this);
			this.animation = new LOR4Animation(this);
			//Members.SetParentSequence(this);
			//string theExtension = lineIn.EndSubstring(3).ToLower();
			string theExtension = Fyle.Extension(lineIn).ToLower();

			if ((theExtension == LOR4Admin.EXT_LAS) || (theExtension == LOR4Admin.EXT_LMS))
			{
				if (Fyle.Exists(lineIn))
				{
					ReadSequenceFile(lineIn);
				}
			}
		}
		#endregion



		#region iLOR4Member Interface

		public override int Centiseconds
		{
			get
			{
				// Return the longest track
				int cs = myCentiseconds;
				for (int t = 0; t < Tracks.Count; t++)
				{
					LOR4Track trk = Tracks[t];
					if (trk.Centiseconds > cs)
					{
						if (trk.Centiseconds > LOR4Admin.MINCentiseconds)
						{
							if (trk.Centiseconds < LOR4Admin.MAXCentiseconds)
							{
								cs = trk.Centiseconds;
							}
						}
					}
				}
				myCentiseconds = cs;
				return cs;
			}
			set
			{
				// LOR allows different tracks to have different times
				// (which doesn't make much sense to me)
				// So just make sure Sequence.Centiseconds is long enough to hold longest track
				// To change (fix) the number of centiseconds for everthing to a consistant length, use SetTotalTime() function below
				if (value > LOR4Admin.MAXCentiseconds)
				{
					string m = "WARNING!! Setting Centiseconds to more than 60 minutes!  Are you sure?";
					Fyle.WriteLogEntry(m, "Warning");
					if (Fyle.DebugMode)
					{
						System.Diagnostics.Debugger.Break();
					}
				}
				else
				{
					if (value < LOR4Admin.MINCentiseconds)
					{
						string m = "WARNING!! Setting Centiseconds to less than 1 second!  Are you sure?";
						Fyle.WriteLogEntry(m, "Warning");
						if (Fyle.DebugMode)
						{
							//System.Diagnostics.Debugger.Break();
						}
					}
					else
					{
						for (int t = 0; t < Tracks.Count; t++)
						{
							LOR4Track trk = Tracks[t];
							if (trk.Centiseconds > value)
							{
								trk.Centiseconds = value;
							}
						}
						myCentiseconds = value;
						MakeDirty(true);
					}
				}
			}
		}

		public void SetTotalTime(int newCentiseconds)
		{
			if (newCentiseconds > LOR4Admin.MAXCentiseconds)
			{
				string m = "WARNING!! Setting Centiseconds to more than 60 minutes!  Are you sure?";
				Fyle.WriteLogEntry(m, "Warning");
				if (Fyle.DebugMode)
				{
					System.Diagnostics.Debugger.Break();
				}
			}
			else
			{
				if (newCentiseconds < LOR4Admin.MINCentiseconds)
				{
					string m = "WARNING!! Setting Centiseconds to less than 1 second!  Are you sure?";
					Fyle.WriteLogEntry(m, "Warning");
					if (Fyle.DebugMode)
					{
						System.Diagnostics.Debugger.Break();
					}
				}
				else
				{
					CentiFix(newCentiseconds);
					MakeDirty(true);
				}
			}

		}

		public override CheckState SelectedState
		{
			// Reminder to myself-- Sequence.Members should only contain the tracks.
			// Each Track.Members will contain its Channels, Groups, etc.
			// Each of those Group.Members will contain more stuff, and so on, recursively
			// Get will return .Unchecked only if absolutely NO descendants are selected.
			//     will return .Unchecked only if absolutely EVERY descendant is selected.
			//		 will return .Indeterminate if some but not all descendants are selected.
			//! Does not include timing grid selections.  Will not affect orphaned members.
			get
			{ return Members.SelectedState; }
			// If Selected is set to .Indeterminate, absolutely nothing will happen, no selection states will change from current.
			// If Selected is set to .Checked all Tracks and all their members and submembers [everything] will be selected recusively
			// If Selected is set to .Unchecked it will clear and all selections (on everything, recursively)
			//! Does not include timing grid selections.  Will not affect orphaned members.
			set
			{
				base.SelectedState = value;
				Members.SelectedState = value;
			}
		}

		public override LOR4MemberType MemberType
		{
			get
			{
				return LOR4MemberType.Sequence;
			}
		}


		public override string LineOut()
		{
			string ret = "";
			// Implemented primarily for compatibility with 'iLOR4Member' interface
			//TODO: make this return something, say maybe the first few lines of the file...?
			return ret;
		}

		public override void Parse(string lineIn)
		{
			ReadSequenceFile(lineIn);
		}



		#endregion

		public string filename
		{
			get
			{
				return info.filename;
			}
			set
			{
				info.filename = value;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources
				if (writer != null)
				{
					writer.Close();
				}
			}
			// free native resources
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}




		public LOR4SequenceType LOR4SequenceType
		{
			get
			{
				return info.sequenceType;
			}
			set
			{
				info.sequenceType = value;
			}
		}

		//////////////////////////////////////////////////
		//                                             //
		//   * *    R E A D   S E Q U E N C E   * *   //
		//                                           //
		//////////////////////////////////////////////
		#region Read Sequence

		public int ReadSequenceFile(string existingFileName)
		{
			return ReadSequenceFile(existingFileName, false);
		}

		public int ReadSequenceFile(string existingFileName, bool noEffects)
		{
			errorStatus = 0;
			string lineIn; // line read in (does not get modified)
			string xmlInfo = "";
			int li = LOR4Admin.UNDEFINED; // positions of certain key text in the line
																		//LOR4Track trk = new Track();
			LOR4SequenceType st = LOR4SequenceType.Undefined;
			string creation = "";
			DateTime modification;
			bool hasEffects = false;

			LOR4Channel lastChannel = null;
			LOR4RGBChannel lastRGBchannel = null;
			LOR4ChannelGroup lastGroup = null;
			LOR4Cosmic lastCosmic = null;
			LOR4Track lastTrack = null;
			LOR4Timings lastGrid = null;
			LOR4LoopLevel lastll = null;
			LOR4AnimationRow lastAniRow = null;
			//LOR4Membership lastMembership = null;

			string ext = Fyle.Extension(existingFileName).ToLower();
			if (ext == LOR4Admin.EXT_LMS) st = LOR4SequenceType.Musical;
			if (ext == LOR4Admin.EXT_LAS) st = LOR4SequenceType.Animated;
			if (ext == LOR4Admin.EXT_LEE) st = LOR4SequenceType.ChannelConfig;
			LOR4SequenceType = st;

			Clear(true);

			info.file_accessed = File.GetLastAccessTime(existingFileName);
			info.file_created = File.GetCreationTime(existingFileName);
			info.file_saved = File.GetLastWriteTime(existingFileName);

			const string ERRproc = " in Sequence:ReadSequenceFile(";
			const string ERRgrp = "), on Line #";
			// const string ERRitem = ", at position ";
			const string ERRline = ", Code Line #";


			try
			{
				reader = new StreamReader(existingFileName);
				// First Sanity Check, does it have ANY lines?
				if (!reader.EndOfStream)
				{
					lineIn = reader.ReadLine();
					lineCount++;
					xmlInfo = lineIn;
					// Sanity Check #2, is it an XML file?  First line should say
					if (lineIn.Substring(0, 6) != "<?xml ")
					{
						errorStatus = ERROR_NotXML;
					}
					else
					{
						lineIn = reader.ReadLine();
						lineCount++;
						// Next sanity check, is it a sequence?  Second line should say
						if (lineIn.Substring(0, 14) != "<sequence save")
						{
							errorStatus = ERROR_NotSequence;
						}
						else
						{
							// Nest Sanity Checks, does it have a 'SaveFileVersion' and is it '14' or higher
							//   (SaveFileVersion="14" means it cane from LOR Sequence Editor ver 4.x)
							if ((info.saveFileVersion < 1) || (info.saveFileVersion > 14))
							{
								errorStatus = ERROR_UnsupportedVersion;
							}
							else
							{
								// Save sequence information (author, artist, created...)
								info.infoLine = lineIn;
								info = new LOR4SeqInfo(this, lineIn);
								creation = info.createdAt;

								// Save this for later, as they will get changed as we populate the file
								modification = info.lastModified;
								info.filename = existingFileName;

								//myName = Path.GetFileName(existingFileName);
								info.xmlInfo = xmlInfo;
								// Next sanity check, do we still have more lines
								if (reader.EndOfStream)
								{
									errorStatus = ERROR_PrematureEnd;
								}
								else
								{
									lineIn = reader.ReadLine();
									lineCount++;
									// Next sanity check, does it have channels?  Third line should say
									li = LOR4Admin.ContainsKey(lineIn, STARTchannels);
									if (li < 0)
									{
										errorStatus = ERROR_NoChannels;
									}
									else
									{
										// Next sanity check, do we have more lines?
										while (!reader.EndOfStream)
										{
											lineIn = reader.ReadLine();
											lineCount++;
											// Sanity Check #5b, 4th line should be a channel (the first one)
											if (lineCount == 4)
											{
												li = LOR4Admin.ContainsKey(lineIn, STARTchannel);
												if (li == 0)
												{ errorStatus = ERROR_NoChannels; }
											}

											if (errorStatus == ERROR_NONE)
											{
												// All sanity checks passed
												// * PARSE LINES
												try // 2nd Try
												{
													//?   (If the order of these sections seems weird, its because they are in order of
													//?     most likely, to least likley)
													//       The largest number of lines will probably be for effects and timings,
													//       The next largest number of lines will probably be regular channels
													//       Next largest will likely be RGB channels, followed by groups
													//       followed by cosmic devices, then tracks, and finally timing grids
													//li = lineIn.IndexOf(STARTchannel);
													li = LOR4Admin.ContainsKey(lineIn, STARTchannel);
													if (li > 0)
													{
														//!///////////////////////////////////////
														//!//  * * *  REGULAR CHANNELS  * * *  //
														//!////////////////////////////////////
														#region Regular Channel
														lastChannel = CreateNewChannel(lineIn);
														// If line ends in "/> it has NO effects (includes slash)(empty channel)
														//   (or if line ends in "> it does have effects (no slash)
														if (!noEffects)
														{
															hasEffects = (lineIn.IndexOf("\"/>") < 0);
															if (hasEffects)
															{
																ReadEffects(lastChannel);
															}
															#endregion // Regular Channel
														} // end effects, or not
													} // end is a channel
													else // Not a regular channel
													{
														li = LOR4Admin.ContainsKey(lineIn, STARTrgbChannel);
														if (li > 0)
														{
															//!///////////////////////////////////
															//!//  * * *  RGB CHANNELS  * * *  //
															//!/////////////////////////////////
															#region RGB Channel
															lastRGBchannel = CreateNewRGBChannel(lineIn);
															lineIn = reader.ReadLine();
															lineCount++;
															lineIn = reader.ReadLine();
															lineCount++;

															// RED
															int csi = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
															LOR4Channel ch = (LOR4Channel)AllMembers.BySavedIndex[csi];
															lastRGBchannel.redChannel = ch;
															ch.rgbChild = LOR4RGBChild.Red;
															ch.rgbParent = lastRGBchannel;
															lineIn = reader.ReadLine();
															lineCount++;

															// GREEN
															csi = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
															ch = (LOR4Channel)AllMembers.BySavedIndex[csi];
															lastRGBchannel.grnChannel = ch;
															ch.rgbChild = LOR4RGBChild.Green;
															ch.rgbParent = lastRGBchannel;
															lineIn = reader.ReadLine();
															lineCount++;

															// BLUE
															csi = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
															ch = (LOR4Channel)AllMembers.BySavedIndex[csi];
															lastRGBchannel.bluChannel = ch;
															ch.rgbChild = LOR4RGBChild.Blue;
															ch.rgbParent = lastRGBchannel;
															#endregion // RGB Channel
														}
														else  // Not an RGB Channel
														{
															//!/////////////////////////////////////
															//!//  * * *  CHANNEL GROUPS  * * *  //
															//!///////////////////////////////////
															//li = lineIn.IndexOf(STARTchannelGroup);
															li = LOR4Admin.ContainsKey(lineIn, STARTchannelGroup);
															if (li > 0)
															{
																#region Channel Group
																lastGroup = CreateNewChannelGroup(lineIn);
																ReadGroupMembers(lastGroup);
															}
															#endregion // Channel Group
															else
															{
																//!///////////////////////////////////////////
																//!//  * * *  COSMIC COLOR DEVICES  * * *  //
																//!/////////////////////////////////////////
																li = LOR4Admin.ContainsKey(lineIn, STARTcosmic);
																if (li > 0)
																{
																	////////////////////////////////
																	///   COSMIC COLOR DEVICE   ///
																	//////////////////////////////
																	#region Cosmic Color Device
																	lastCosmic = CreateNewCosmicDevice(lineIn);
																	ReadCosmicMembers(lastCosmic);
																	#endregion // Cosmic Color Device
																}
																else // Not a Cosmic Device
																{
																	//!/////////////////////////////
																	//!//  * * *  TRACKS  * * *  //
																	//!///////////////////////////

																	//BUG Tracks are [apparently] getting added twice

																	//li = lineIn.IndexOf(STARTtrack);
																	li = LOR4Admin.ContainsKey(lineIn, STARTtrack);
																	if (li > 0)
																	{
																		//////////////////
																		///   TRACK   ///
																		////////////////
																		#region Track
																		lastTrack = CreateNewTrack(lineIn);
																		//! TimingGrid SaveID gets set here during Parse() [When creating new Track from LIneIn)
																		//!  But the Track won't get it's reference to the TimingGrid object until later.
																		//!    That's 'cuz we haven't read the Timing Grids yet from the file!
																		//!      CORRECTION: in Showtime version 3 and earlier, the timing grids DO come before
																		//!       the tracks, version 4 they come after, but that doesn't affect refernceing them.
																		ReadTrackMembers(lastTrack);
																		#endregion // Track
																	}
																	else // not a track
																	{
																		//!///////////////////////////////////
																		//!//  * * *  TIMING GRIDS  * * *  //
																		//!/////////////////////////////////
																		//li = lineIn.IndexOf(STARTtimingGrid);
																		li = LOR4Admin.ContainsKey(lineIn, STARTtimingGrid);
																		if (li > 0)
																		{
																			////////////////////////
																			///   TIMING GRID   ///
																			//////////////////////
																			#region Timing Grid
																			lastGrid = CreateNewTimingGrid(lineIn);
																			if (lastGrid.TimingGridType == LOR4TimingGridType.Freeform)
																			{
																				ReadTimings(lastGrid);
																			}
																			#endregion
																		}
																		else // Not a timing grid
																		{
																			//!//////////////////////////////////
																			//!//  * * *  LOOPS and    * * *  //
																			//!//  * * *  ANIMATIONS  * * *  //
																			//!///////////////////////////////
																			//! Loop Levels
																			//li = lineIn.IndexOf(STARTloopLevel);
																			li = LOR4Admin.ContainsKey(lineIn, STARTloopLevel);
																			if (li > 0)
																			{
																				lastll = lastTrack.AddLoopLevel(lineIn);
																			}
																			else // not a loop level
																			{
																				//! Loops
																				//li = lineIn.IndexOf(STARTloop);
																				li = LOR4Admin.ContainsKey(lineIn, STARTloop);
																				if (li > 0)
																				{
																					lastll.AddLoop(lineIn);
																				}
																				else // not a loop
																				{
																					//! Animation Rows
																					//li = lineIn.IndexOf(STARTaniRow);
																					li = LOR4Admin.ContainsKey(lineIn, STARTaniRow);
																					if (li > 0)
																					{
																						lastAniRow = animation.AddRow(lineIn);
																					}
																					else
																					{
																						//! Animation Columns
																						//li = lineIn.IndexOf(STARTaniCol);
																						li = LOR4Admin.ContainsKey(lineIn, STARTaniCol);
																						if (li > 1)
																						{
																							lastAniRow.AddColumn(lineIn);
																						} // end animationColumn
																						else
																						{
																							//! Animation
																							//li = lineIn.IndexOf(LOR4Admin.STFLD + TABLEanimation + LOR4Admin.SPC);
																							li = LOR4Admin.ContainsKey(lineIn, LOR4Admin.STFLD + TABLEanimation + LOR4Admin.SPC);
																							if (li > 0)
																							{
																								animation = new LOR4Animation(this, lineIn);

																							} // end this is an Animation Table (or not)
																						} // end this is an Animation Column (or not)
																					} // end this is an Animation Row (or not)
																				} // end this is a Loop Start (or not)
																			} // end this is a Loop Level (or not)
																		} // end this is a Timing Grid (or not)
																	} // end this is a Track (or not)
																}  // end this is a Cosmic Color Device (or not)
															} // end this is a Channel Group (or not)
														} // end this is an RGB channel (or not)
													} // end this is a regular Channel (or not)
												} // end 2nd try/catch
												catch (Exception ex)
												{
													StackTrace strx = new StackTrace(ex, true);
													StackFrame sf = strx.GetFrame(strx.FrameCount - 1);
													string emsg = ex.Message + LOR4Admin.CRLF;
													emsg += "at Sequence.ReadSequence()" + LOR4Admin.CRLF;
													emsg += "File:" + existingFileName + LOR4Admin.CRLF;
													emsg += "on line " + lineCount.ToString() + " at position " + li.ToString() + LOR4Admin.CRLF;
													emsg += "Line Is:" + lineIn + LOR4Admin.CRLF;
													emsg += "in code line " + sf.GetFileLineNumber() + LOR4Admin.CRLF;
													emsg += "Last SavedIndex = " + AllMembers.HighestSavedIndex.ToString();
													info.LastError.fileLine = lineCount;
													info.LastError.linePos = li;
													info.LastError.codeLine = sf.GetFileLineNumber();
													info.LastError.errName = ex.ToString();
													info.LastError.errMsg = emsg;
													info.LastError.lineIn = lineIn;

#if DEBUG
													System.Diagnostics.Debugger.Break();
#endif
													Fyle.WriteLogEntry(emsg, LOR4Admin.LOG_Error);
													if (debugMode)
													{
														DialogResult dr1 = MessageBox.Show(emsg, "Error Reading Sequence File", MessageBoxButtons.OK, MessageBoxIcon.Error);
														System.Diagnostics.Debugger.Break();
													} // end if debug mode
												} // end second catch
													// Restore these to the values we captured when first reading the file info header
												info.createdAt = creation;
												info.lastModified = info.file_saved;
												MakeDirty(false);
												AllMembers.ReIndex();
											} // end still no errors
										} // still more lines (at least 4)
									} // end third line is start of channels
								} // end still has more lines (at least 3)
							} // end has a valid 'saveVersion'
						} // end second line is a sequence
					} // end first line was xml info
				} // end has a first line
				reader.Close();
			} // end first try
			catch (Exception ex)
			{
				StackTrace strc = new StackTrace(ex, true);
				StackFrame sf = strc.GetFrame(strc.FrameCount - 1);
				string emsg = ex.ToString();
				emsg += ERRproc + existingFileName + ERRgrp + "none";
				emsg += ERRline + sf.GetFileLineNumber();
#if DEBUG
				System.Diagnostics.Debugger.Break();
#endif
				Fyle.WriteLogEntry(emsg, LOR4Admin.LOG_Error);
				if (debugMode)
				{
					DialogResult dr2 = MessageBox.Show(emsg, "Error Opening Sequence File", MessageBoxButtons.OK, MessageBoxIcon.Error);
					System.Diagnostics.Debugger.Break();
				}
				errorStatus = ERROR_CantOpen;
			} // end first catch


			if (errorStatus < 100)
			{
				info.filename = existingFileName;
				//! for debugging
				//string sMsg = summary();
				//MessageBox.Show(sMsg, "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			return errorStatus;
		} // end ReadSequenceFile

		//!//////////////////////////////
		//!//  * * *  EFFECTS  * * *  //
		//!////////////////////////////
		private int ReadEffects(LOR4Channel channel)
		{
			int entryCount = 0;
			string effIn = reader.ReadLine();
			int li = LOR4Admin.ContainsKey(effIn, STARTeffect);
			while (li > 0)
			{
				channel.AddEffect(effIn);
				effIn = reader.ReadLine();
				lineCount++;
				entryCount++;
				li = LOR4Admin.ContainsKey(effIn, STARTeffect);
			}
			return entryCount;
		}

		//!//////////////////////////////
		//!//  * * *  TIMINGS  * * *  //
		//!////////////////////////////
		private int ReadTimings(LOR4Timings grid)
		{
			int entryCount = 0;
			string timeIn = "";
			timeIn = reader.ReadLine();
			int li = LOR4Admin.ContainsKey(timeIn, STARTtiming);
			while (li > 0)
			{
				int t = LOR4Admin.getKeyValue(timeIn, LOR4Admin.FIELDcentisecond); // Note: non-plural, no 's' at the end
				grid.AddTiming(t);
				timeIn = reader.ReadLine();
				lineCount++;
				entryCount++;
				li = LOR4Admin.ContainsKey(timeIn, STARTtiming);
			}
			return entryCount;
		}

		//!////////////////////////////////////
		//!//  * * *  TRACK MEMBERS  * * *  //
		//!//////////////////////////////////
		private int ReadTrackMembers(LOR4Track track)
		{
			int entryCount = 0;
			int li = 0;
			string itemIn = "";
			itemIn = reader.ReadLine(); // "<channels>"
			lineCount++;
			itemIn = reader.ReadLine();
			lineCount++;
			li = LOR4Admin.ContainsKey(itemIn, STARTtrackItem);
			while (li > 0)
			{
				int isi = LOR4Admin.getKeyValue(itemIn, LOR4Admin.FIELDsavedIndex);
				//if (isi == 2189) System.Diagnostics.Debugger.Break();
				if (isi <= AllMembers.HighestSavedIndex)
				{
					iLOR4Member SIMem = AllMembers.BySavedIndex[isi];
					if (SIMem != null)
					{
						track.Members.Add(SIMem);
						entryCount++;
					}
					else
					{
						///WTF why wasn't if found?!?!
						System.Diagnostics.Debugger.Break();
					}
				}
				else
				{
					///WTF why is savedIndex too high?!?!
					System.Diagnostics.Debugger.Break();
				}
				itemIn = reader.ReadLine();
				lineCount++;
				li = LOR4Admin.ContainsKey(itemIn, STARTtrackItem);
			}
			return entryCount;
		}

		//!////////////////////////////////////
		//!//  * * *  GROUP MEMBERS  * * *  //
		//!//////////////////////////////////
		private int ReadGroupMembers(LOR4ChannelGroup group)
		{
			// Wrapper of sorts
			return ReadGroupMembers(group.Members);
		}

		private int ReadCosmicMembers(LOR4Cosmic cosmic)
		{
			// Cosmic Color Devices are basically the same thing as Channel Groups
			return ReadGroupMembers(cosmic.Members);
		}

		private int ReadGroupMembers(LOR4Membership members)
		{
			int entryCount = 0;
			int li = 0;
			string itemIn = "";
			itemIn = reader.ReadLine(); // "<channelGroups>"
			lineCount++;
			itemIn = reader.ReadLine();
			lineCount++;

			li = LOR4Admin.ContainsKey(itemIn, TABLEchannelGroup + LOR4Admin.FIELDsavedIndex);
			while (li > 0)
			{
				int isl = LOR4Admin.getKeyValue(itemIn, LOR4Admin.FIELDsavedIndex);
				if (isl <= AllMembers.HighestSavedIndex)
				{
					iLOR4Member SIMem = AllMembers.BySavedIndex[isl];
					if (SIMem != null)
					{
						members.Add(AllMembers.BySavedIndex[isl]);
						entryCount++;
					}
					else
					{
						///WTF why wasn't if found?!?!
						System.Diagnostics.Debugger.Break();
					}
				}
				else
				{
					///WTF why is savedIndex too high?!?!
					System.Diagnostics.Debugger.Break();
				}
				itemIn = reader.ReadLine();
				lineCount++;
				li = LOR4Admin.ContainsKey(itemIn, TABLEchannelGroup + LOR4Admin.FIELDsavedIndex);
			}
			return entryCount;
		}



		#endregion

		////////////////////////////////////////////////////
		//                                               //
		//   * *    W R I T E   S E Q U E N C E   * *   //
		//                                             //
		////////////////////////////////////////////////
		#region Write Sequence

		public int WriteSequenceFile(string newFileName)
		{
			return WriteSequenceFile_DisplayOrder(newFileName, false, false);
		}

		public int WriteSequenceFile_DisplayOrder(string newFileName)
		{
			return WriteSequenceFile_DisplayOrder(newFileName, false, false);
		}

		public int WriteSequenceFile_DisplayOrder(string newFileName, bool selectedOnly, bool noEffects)
		{
			List<int> destSIs = new List<int>();
			//int altSI = LOR4Admin.UNDEFINED;
			updatedTrack[] updatedTracks = new updatedTrack[Tracks.Count];
			//bySavedIndex.AltHighestSavedIndex = LOR4Admin.UNDEFINED;
			//bySavedIndex.altSaveID = LOR4Admin.UNDEFINED;
			//altSavedIndexes = null;
			AllMembers.ResetWritten();
			TimingsResetWritten();
			//Array.Resize(ref altSavedIndexes, highestSavedIndex + 3);
			//Array.Resize(ref altSaveIDs, TimingGrids.Count + 1);
			string ext = Fyle.Extension(newFileName).ToLower();
			bool channelConfig = false;
			if (ext == LOR4Admin.EXT_LCC) channelConfig = true;
			if (channelConfig) noEffects = true;
			//TODO: implement channelConfig flag to write just a channel config file

			// Clear any 'Written' flags from a previous save
			ClearWrittenFlags();

			// Write the Timing Grids
			if (WriteSelectedGridsOnly)
			{
				//! First, A Track->Timings renumbering
				// Timing Grids do not get Written to the file yet
				// But we must renumber the saveIDs
				// Assign new altSaveIDs in the order they appear in the Tracks
				foreach (LOR4Track theTrack in Tracks)
				{
					// Note the double negative here- Include it if it is selected or indeterminate
					if ((!selectedOnly) || (theTrack.SelectedState != CheckState.Unchecked))
					{
						if (theTrack.timingGrid == null)
						{
							// File integrity check, should never happen under normal circumstances with a sequence
							// But it happens when starting from a channel config file
							if (TimingGrids.Count > 0)
							{
								// just use the first one
								theTrack.timingGrid = TimingGrids[0];
							}
							else
							{
								// Create one
								LOR4Timings ntg = CreateNewTimingGrid("Fixed Grid .05");
								ntg.spacing = 5;
								theTrack.timingGrid = ntg;
							}
						}
						int asi = AssignNextAltSaveID(theTrack.timingGrid);
					}
				}
				for (int tg = 0; tg < TimingGrids.Count; tg++)
				{
					LOR4Timings theGrid = TimingGrids[tg];
					// Any remaining timing grids that are Selected, but not used by any Tracks
					if ((!selectedOnly) || (theGrid.SelectedState != CheckState.Unchecked))
					{
						if (theGrid.AltSaveID == LOR4Admin.UNDEFINED)
						{
							int asi = AssignNextAltSaveID(theGrid);
							theGrid.AltSaveID = asi;
							//altSaveIDs[tg] = altSaveID;
						}
					}
				}
			}
			else // if (WriteSelectedGridsOnly)
			{
				// ALWAYS write ALL timing Grids, and keep same numerical order
				for (int tg = 0; tg < TimingGrids.Count; tg++)
				{
					LOR4Timings theGrid = TimingGrids[tg];
					if (theGrid.AltSaveID == LOR4Admin.UNDEFINED)
					{
						int asi = AssignNextAltSaveID(theGrid);
						theGrid.AltSaveID = asi;
					}
				}
			}

			//! NOW it's time to write the file
			// Write the first line of the new sequence, containing the XML info
			WriteSequenceStart(newFileName);

			// Start with Channels (regular, RGB, and Groups)
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);

			// Loop thru Tracks and write the items (details) in the order they appear
			foreach (LOR4Track theTrack in Tracks)
			{
				if ((!selectedOnly) || (theTrack.SelectedState != CheckState.Unchecked))
				{
					WriteListItems(theTrack.Members, selectedOnly, noEffects, LOR4MemberType.Items);
				}
			}
			// All Channels should now be Written, close this section
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.FINTBL + LOR4Admin.TABLEchannel + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);

			// TIMING GRIDS
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.STFLD + LOR4Sequence.TABLEtimingGrid + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);
			foreach (LOR4Timings theGrid in TimingGrids)
			{
				// TIMING GRIDS
				if ((!selectedOnly) || (theGrid.SelectedState != CheckState.Unchecked))
				{
					//! Does not seem to be numbering them correctly, or saving them with the tracks
					WriteTimingGrid(theGrid);
				}
			}
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.FINTBL + LOR4Sequence.TABLEtimingGrid + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);

			// TRACKS
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.STFLD + TABLEtrack + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);
			// Loop thru Tracks
			foreach (LOR4Track theTrack in Tracks)
			{
				if ((!selectedOnly) || (theTrack.SelectedState != CheckState.Unchecked))
				{
					// Items in the track have already been written
					// This writes the track info itself including its member list
					// and loop levels
					//! Timing Grid numbering is off, and tracks are not being associated with grids!
					WriteTrack(theTrack, selectedOnly, LOR4MemberType.Items);
				}
			}
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.FINTBL + TABLEtrack + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);

			WriteSequenceClose();

			errorStatus = RenameTempFile(newFileName);
			if (filename.Length < 3)
			{
				//filename = newFileName;
				info.filename = newFileName;
			}
			MakeDirty(false);
			return errorStatus;
		} // end WriteSequenceFileInDisplayOrder

		private int WriteSequenceStart(string newFileName)
		{
			//string lineOut = "";

			errorStatus = 0;
			lineCount = 0;
			newFilename = newFileName;
			if (!Directory.Exists(tempWorkPath)) Directory.CreateDirectory(tempWorkPath);
			tempFileName = tempWorkPath + Path.GetFileNameWithoutExtension(newFilename) + Fyle.EXT_TMP;
			writer = new StreamWriter(tempFileName);

			// Write the first line of the new sequence, containing the XML info
			lineOut = info.xmlInfo;
			writer.WriteLine(lineOut);
			//string createdAt = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
			lineOut = info.LineOut();
			writer.WriteLine(lineOut);

			lineOut = "";

			return errorStatus;
		}

		public int WriteChannel(LOR4Channel theChannel)
		{
			return WriteChannel(theChannel, false);
		}

		public int WriteChannel(LOR4Channel theChannel, bool noEffects)
		{
			//theChannel.AltSavedIndex = bySavedIndex.GetNextAltSavedIndex(theChannel.SavedIndex);
			if (theChannel.AltSavedIndex < 0) // (!Written)
			{
				int altSI = AssignNextAltSavedIndex(theChannel);
				theChannel.AltSavedIndex = altSI;
				lineOut = theChannel.LineOut(noEffects);
				writer.WriteLine(lineOut);
				//theChannel.Written = true;
			}
			else
			{
				// Why is this being called, didn't I already check the Written state?
				//System.Diagnostics.Debugger.Break();
			}
			return theChannel.AltSavedIndex;
		}

		private int WriteRGBchannel(LOR4RGBChannel theRGBchannel)
		{
			return WriteRGBchannel(theRGBchannel, false, false, LOR4MemberType.Items);
		}

		private int WriteRGBchannel(LOR4RGBChannel theRGBchannel, bool selectedOnly, bool noEffects, LOR4MemberType itemTypes)
		{
			if (!theRGBchannel.Written)
			{
				if ((!selectedOnly) || (theRGBchannel.SelectedState != CheckState.Unchecked))
				{
					if ((itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.Channel))
					{
						//theRGBchannel.LineOut(selectedOnly, noEffects, LOR4MemberType.Channel);
						//lineOut = theRGBchannel.redChannel.LineOut(noEffects);
						//lineOut += LOR4Admin.CRLF + theRGBchannel.grnChannel.LineOut(noEffects);
						//lineOut += LOR4Admin.CRLF + theRGBchannel.bluChannel.LineOut(noEffects);
						WriteChannel(theRGBchannel.redChannel, noEffects);
						WriteChannel(theRGBchannel.grnChannel, noEffects);
						WriteChannel(theRGBchannel.bluChannel, noEffects);
						//writer.WriteLine(lineOut);
					}

					if ((itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.RGBChannel))
					{
						//theRGBchannel.AltSavedIndex = bySavedIndex.GetNextAltSavedIndex(theRGBchannel.SavedIndex);
						int altSI = AssignNextAltSavedIndex(theRGBchannel);
						lineOut = theRGBchannel.LineOut(selectedOnly, noEffects, LOR4MemberType.RGBChannel);
						writer.WriteLine(lineOut);
						//theRGBchannel.Written = true;
					}
				}
			}
			return theRGBchannel.AltSavedIndex;
		} // end writergbChannel

		private int WriteChannelGroup(LOR4ChannelGroup theGroup)
		{
			return WriteChannelGroup(theGroup, false, false, LOR4MemberType.Items);
		}

		private int WriteChannelGroup(LOR4ChannelGroup theGroup, bool selectedOnly, bool noEffects, LOR4MemberType itemTypes)
		{
			// May be called recursively (because groups can contain groups)
			List<int> altSIs = new List<int>();
			//if (theGroup.Name.IndexOf("ng Parts") > 0) System.Diagnostics.Debugger.Break();
			//if (theGroup.Name.IndexOf("lyphonic") > 0) System.Diagnostics.Debugger.Break();

			if (!theGroup.Written)
			{
				if ((!selectedOnly) || (theGroup.SelectedState != CheckState.Unchecked))
				{
					if ((itemTypes == LOR4MemberType.Items) ||
						(itemTypes == LOR4MemberType.Channel) ||
						(itemTypes == LOR4MemberType.RGBChannel))
					{
						// WriteListItems may in turn call this procedure (WriteChannelGroup) for any Child Channel Groups
						// Therefore making it recursive at this point
						altSIs = WriteListItems(theGroup.Members, selectedOnly, noEffects, itemTypes);
					}

					if ((itemTypes == LOR4MemberType.Items) ||
						(itemTypes == LOR4MemberType.ChannelGroup))
					{
						//theGroup.AltSavedIndex = bySavedIndex.GetNextAltSavedIndex(theGroup.SavedIndex);
						//if (altSIs.Count > 0)
						//{
						int altSI = AssignNextAltSavedIndex(theGroup);
						theGroup.AltSavedIndex = altSI;
						//lineOut = theGroup.LineOut(selectedOnly);

						lineOut = LOR4Admin.LEVEL2 + LOR4Admin.STFLD + LOR4Sequence.TABLEchannelGroupList;
						lineOut += LOR4Admin.FIELDtotalCentiseconds + LOR4Admin.FIELDEQ + theGroup.Centiseconds.ToString() + LOR4Admin.ENDQT;
						lineOut += LOR4Admin.FIELDname + LOR4Admin.FIELDEQ + LOR4Admin.XMLifyName(theGroup.Name) + LOR4Admin.ENDQT;
						lineOut += LOR4Admin.FIELDsavedIndex + LOR4Admin.FIELDEQ + altSI.ToString() + LOR4Admin.ENDQT;
						lineOut += LOR4Admin.FINFLD;
						writer.WriteLine(lineOut);

						WriteItemsList(theGroup.Members, selectedOnly, itemTypes);

						lineOut = LOR4Admin.LEVEL2 + LOR4Admin.FINTBL + LOR4Sequence.TABLEchannelGroupList + LOR4Admin.FINFLD;
						writer.WriteLine(lineOut);
						//}

						//curSavedIndex++;
						// copy the original SavedIndex to the altSavedIndexes at the altHighestSavedIndex position
						//altSavedIndexes[altHighestSavedIndex] = savedIndexes[theGroup.SavedIndex].altCopy();
						// cross reference the new AltSavedIndex to the original SavedIndex
						//altSavedIndexes[altHighestSavedIndex].AltSavedIndex = theGroup.SavedIndex;
						// and cross reference the original saved index to the new AltSavedIndex
						//savedIndexes[theGroup.SavedIndex].AltSavedIndex = altHighestSavedIndex;
					}
				}
			}
			return theGroup.AltSavedIndex;
		} // end writeChannelGroup

		private int WriteTrack(LOR4Track theTrack, bool selectedOnly, LOR4MemberType itemTypes)
		{
			string lineOut = LOR4Admin.LEVEL2 + LOR4Admin.STFLD + LOR4Sequence.TABLEtrack;
			//! LOR writes it with the Name last
			// In theory, it shouldn't matter
			//if (Name.Length > 1)
			//{
			//	ret += LOR4Admin.SPC + FIELDname + LOR4Admin.FIELDEQ + Name + LOR4Admin.ENDQT;
			//}
			lineOut += LOR4Admin.FIELDtotalCentiseconds + LOR4Admin.FIELDEQ + theTrack.Centiseconds.ToString() + LOR4Admin.ENDQT;
			int altID = 0;
			if (theTrack.timingGrid == null)
			{
				if (TimingGrids.Count == 0)
				{
					Fyle.BUG("Why are there no Timing Grids?");
				}
				else
				{
					theTrack.timingGrid = TimingGrids[0];
					altID = theTrack.timingGrid.AltSaveID;
				}
			}
			// if track is selected, but it's timing grid isn't, default to grid 0
			if (altID < 0) altID = 0;
			lineOut += LOR4Admin.SPC + LOR4Sequence.TABLEtimingGrid + LOR4Admin.FIELDEQ + altID.ToString() + LOR4Admin.ENDQT;
			// LOR writes it with the Name last
			if (theTrack.Name.Length > 1)
			{
				lineOut += LOR4Admin.FIELDname + LOR4Admin.FIELDEQ + LOR4Admin.XMLifyName(theTrack.Name) + LOR4Admin.ENDQT;
			}
			lineOut += LOR4Admin.FINFLD;
			//int siOld = LOR4Admin.UNDEFINED;
			//int siAlt = LOR4Admin.UNDEFINED;
			writer.WriteLine(lineOut);

			WriteItemsList(theTrack.Members, selectedOnly, itemTypes);

			// Write out any LoopLevels in this track
			lineOut = "";
			if (theTrack.loopLevels.Count > 0)
			{
				lineOut += LOR4Admin.LEVEL3 + LOR4Admin.STFLD + LOR4Sequence.TABLEloopLevels + LOR4Admin.FINFLD + LOR4Admin.CRLF;
				foreach (LOR4LoopLevel ll in theTrack.loopLevels)
				{
					lineOut += ll.LineOut() + LOR4Admin.CRLF;
				}
				lineOut += LOR4Admin.LEVEL3 + LOR4Admin.FINTBL + LOR4Sequence.TABLEloopLevels + LOR4Admin.FINFLD + LOR4Admin.CRLF;
			}
			else
			{
				lineOut += LOR4Admin.LEVEL3 + LOR4Admin.STFLD + LOR4Sequence.TABLEloopLevels + LOR4Admin.ENDFLD + LOR4Admin.CRLF;
			}

			// finish the track
			lineOut += LOR4Admin.LEVEL2 + LOR4Admin.FINTBL + LOR4Sequence.TABLEtrack + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);


			return theTrack.AltTrackID;
		}

		private List<int> WriteListItems(LOR4Membership itemIDs, bool selectedOnly, bool noEffects, LOR4MemberType itemTypes)
		{
			// NOTE: This writes out all the individual items in a membership list
			// It is recursive, also writing any items in subgroups
			// This does NOT write the list of items, that is handled later by the counterpart to this, 'WriteItemsList'

			int altSaveIndex = LOR4Admin.UNDEFINED;
			List<int> altSIs = new List<int>();
			string itsName = "";  //! for debugging

			//if (itemIDs.Owner.Name.IndexOf("ng Parts") > 0) System.Diagnostics.Debugger.Break();
			//if (itemIDs.Owner.Name.IndexOf("lyphonic") > 0) System.Diagnostics.Debugger.Break();
			//Identity id = null;

			foreach (iLOR4Member item in itemIDs.Items)
			{
				// if (item.Name.IndexOf("lyphonic") > 0) System.Diagnostics.Debugger.Break();

				//id = Members.BySavedIndex[si];
				itsName = item.Name;
				if ((!selectedOnly) || (item.SelectedState != CheckState.Unchecked))
				{
					if (!item.Written)
					{
						if (item.MemberType == LOR4MemberType.Channel)
						{
							// Prevents unnecessary processing of Channels which have already been Written, during RGB channel and group processing
							if ((itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.Channel))
							{
								altSaveIndex = WriteChannel((LOR4Channel)item, noEffects);
								altSIs.Add(altSaveIndex);
							}
						}
						else
						{
							if (item.MemberType == LOR4MemberType.RGBChannel)
							{
								LOR4RGBChannel theRGB = (LOR4RGBChannel)item;
								if ((itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.Channel) || (itemTypes == LOR4MemberType.RGBChannel))
								{
									altSaveIndex = WriteRGBchannel(theRGB, selectedOnly, noEffects, itemTypes);
									altSIs.Add(altSaveIndex);
								}
							}
							else
							{
								if (item.MemberType == LOR4MemberType.ChannelGroup)
								{
									//if (itemTypes == LOR4MemberType.channelGroup)
									//if ((itemTypes == LOR4MemberType.None) ||
									//    (itemTypes == LOR4MemberType.rgbChannel) ||
									//    (itemTypes == LOR4MemberType.channel) ||
									//    (itemTypes == LOR4MemberType.channelGroup))
									// Type NONE actually means ALL in this case
									//{

									//									if (item.Name.IndexOf("lyphonic") > 0) System.Diagnostics.Debugger.Break(); // Break here for debugging if named group is found


									// WriteChannelGroup calls this procedure (WriteListItems) so this is where it gets recursive
									altSaveIndex = WriteChannelGroup((LOR4ChannelGroup)item, selectedOnly, noEffects, itemTypes);
									altSIs.Add(altSaveIndex);
									//}
								} // if ChannelGroup, or not
							} // if RGBChannel, or not
						} // if regular Channel, or not
					} // if not Written
				} // if Selected
			} // loop thru items

			return altSIs;
		}

		private int WriteItemsList(LOR4Membership subMembers, bool selectedOnly, LOR4MemberType itemTypes)
		{
			// NOTE: This writes out the list of items in a membership list.
			// It is NOT recursive.
			// This does NOT write the the individual items, that was handled previously by the counterpart to this, 'WriteListItems'

			//if (subMembers.Owner.Name.IndexOf("ng Parts") > 0) System.Diagnostics.Debugger.Break();
			//if (subMembers.Owner.Name.IndexOf("lyphonic") > 0) System.Diagnostics.Debugger.Break();

			int count = 0;
			string leader = LOR4Admin.LEVEL4 + LOR4Admin.STFLD;


			string lineOut = LOR4Admin.LEVEL3 + LOR4Admin.STFLD;
			if (subMembers.Owner.MemberType == LOR4MemberType.Track)
			{
				lineOut += LOR4Admin.TABLEchannel;
				leader += LOR4Admin.TABLEchannel;
			}
			if (subMembers.Owner.MemberType == LOR4MemberType.ChannelGroup)
			{
				lineOut += LOR4Sequence.TABLEchannelGroup;
				leader += LOR4Sequence.TABLEchannelGroup;
			}
			lineOut += LOR4Admin.PLURAL + LOR4Admin.FINFLD + LOR4Admin.CRLF;

			// Loop thru all items in membership list
			foreach (iLOR4Member subMember in subMembers.Items)
			{
				//if (subID.Name.IndexOf("lyphonic") > 0) System.Diagnostics.Debugger.Break();

				if (subMember.Written)  // The item itself should have already been written!
				{
					CheckState sel = subMember.SelectedState;
					// Note double negative here- will include if Checked or Interderminate
					// Reminder: Order of precedence will evaluate != before ||
					if (!selectedOnly || sel != CheckState.Unchecked)
					{
						if ((itemTypes == subMember.MemberType) || (itemTypes == LOR4MemberType.Items))
						{
							int siAlt = subMember.AltID;
							if (siAlt > LOR4Admin.UNDEFINED)
							{
								lineOut += leader;
								lineOut += LOR4Admin.FIELDsavedIndex + LOR4Admin.FIELDEQ + siAlt.ToString() + LOR4Admin.ENDQT;
								lineOut += LOR4Admin.ENDFLD + LOR4Admin.CRLF;
								count++;
							}
						}
					}
				}
			}

			// Close the list of items
			lineOut += LOR4Admin.LEVEL3 + LOR4Admin.FINTBL;
			if (subMembers.Owner.MemberType == LOR4MemberType.Track)
			{
				lineOut += LOR4Admin.TABLEchannel;
			}
			if (subMembers.Owner.MemberType == LOR4MemberType.ChannelGroup)
			{
				lineOut += LOR4Sequence.TABLEchannelGroup;
			}
			lineOut += LOR4Admin.PLURAL + LOR4Admin.FINFLD;

			writer.WriteLine(lineOut);
			return count;
		}

		public int WriteItem(int SavedIndex)
		{
			return WriteItem(SavedIndex, false, false, LOR4MemberType.Items);
		}

		public int WriteItem(int SavedIndex, bool selectedOnly, bool noEffects, LOR4MemberType theType)
		{
			int ret = LOR4Admin.UNDEFINED;

			iLOR4Member member = AllMembers.BySavedIndex[SavedIndex];
			if (!member.Written)
			{
				// Reminder: Order of precedence will evaluate != before ||
				if (!selectedOnly || member.SelectedState != CheckState.Unchecked)
				{
					LOR4MemberType itemType = member.MemberType;
					if (itemType == LOR4MemberType.Channel)
					{
						LOR4Channel theChannel = (LOR4Channel)member;
						if ((theType == LOR4MemberType.Channel) || (theType == LOR4MemberType.Items))
						{
							ret = WriteChannel(theChannel, noEffects);
						} // end if type
					} // end if Channel
					else
					{
						if (itemType == LOR4MemberType.RGBChannel)
						{
							LOR4RGBChannel theRGB = (LOR4RGBChannel)member;
							ret = WriteRGBchannel(theRGB, selectedOnly, noEffects, theType);
						} // end if RGBChannel
						else
						{
							if (itemType == LOR4MemberType.ChannelGroup)
							{
								LOR4ChannelGroup theGroup = (LOR4ChannelGroup)member;
								ret = WriteChannelGroup(theGroup, selectedOnly, noEffects, theType);
							} // end if RGBChannel
						} // RGBChannel, or not
					} // end a channel, or not
				} // end Selected
			} // end if not Written

			return ret;
		}



		private int WriteTimingGrid(LOR4Timings theGrid)
		{
			return WriteTimingGrid(theGrid, false);
		}

		private int WriteTimingGrid(LOR4Timings theGrid, bool selectedOnly)
		{
			int altSI = AssignNextAltSaveID(theGrid);
			lineOut = theGrid.LineOut();
			writer.WriteLine(lineOut);
			return theGrid.AltSaveID;

		} // end writeTimingGrids

		private void WriteSequenceClose()
		{
			string lineOut = "";

			// Write out Animation info, it it exists
			WriteAnimation();

			// Close the sequence
			lineOut = LOR4Admin.FINTBL + TABLEsequence + LOR4Admin.FINFLD; // "</sequence>";
			writer.WriteLine(lineOut);

			Console.WriteLine(lineCount.ToString() + " Out:" + lineOut);
			Console.WriteLine("");

			// We're done writing the file
			writer.Flush();
			writer.Close();
			info.file_saved = DateTime.Now;
			MakeDirty(false);
		}

		private void WriteAnimation()
		{
			if (animation != null)
			{
				if (animation.sections > 0)
				{
					lineOut = animation.LineOut();
					writer.WriteLine(lineOut);
				}
			} // end if animation != null
		} // end writeAnimation

		private void ClearWrittenFlags()
		{
			foreach (LOR4Channel ch in Channels)
			{
				//ch.Written = false;
				ch.AltSavedIndex = LOR4Admin.UNDEFINED;
			}
			foreach (LOR4RGBChannel rch in RGBchannels)
			{
				//rch.Written = false;
				rch.AltSavedIndex = LOR4Admin.UNDEFINED;
			}
			foreach (LOR4ChannelGroup chg in ChannelGroups)
			{
				//chg.Written = false;
				chg.AltSavedIndex = LOR4Admin.UNDEFINED;
			}
			foreach (LOR4Cosmic dev in CosmicDevices)
			{
				//chg.Written = false;
				dev.AltSavedIndex = LOR4Admin.UNDEFINED;
			}
			foreach (LOR4Track tr in Tracks)
			{
				tr.AltTrackID = LOR4Admin.UNDEFINED;
			}
			foreach (LOR4Timings tg in TimingGrids)
			{
				//tg.Written = false;
				tg.AltSaveID = LOR4Admin.UNDEFINED;
			}
		} // end ClearWrittenFlags

		#endregion


		public override iLOR4Member Parent
		{
			get
			{
				// I am my own parent!
				return this;
			}
		}

		public static string DefaultSequencesPath
		{
			get
			{
				return LOR4Admin.DefaultNonAudioPath;
			}
		}



		//TODO: add RemoveChannel, RemoveRGBchannel, RemoveChannelGroup, and RemoveTrack procedures



		public string GetMemberName(int SavedIndex)
		{
			string nameOut = "";
			iLOR4Member member = AllMembers.BySavedIndex[SavedIndex];
			if (member != null)
			{
				nameOut = member.Name;
			}
			return nameOut;
		}

		public void Clear(bool areYouReallySureYouWantToDoThis)
		{
			if (areYouReallySureYouWantToDoThis)
			{
				// Zero these out from any previous run
				lineCount = 0;
				this.Members = new LOR4Membership(this);
				this.AllMembers = new LOR4Membership(this);
				//this.Members.SetParent(this);
				this.info = new LOR4SeqInfo(this);
				this.animation = new LOR4Animation(this);
				Channels = new List<LOR4Channel>();
				RGBchannels = new List<LOR4RGBChannel>();
				ChannelGroups = new List<LOR4ChannelGroup>();
				CosmicDevices = new List<LOR4Cosmic>();
				Tracks = new List<LOR4Track>();
				TimingGrids = new List<LOR4Timings>();
				//Members.SetParentSequence(this);
				myCentiseconds = 0;
				MakeDirty(false);

			} // end Are You Sure
		} // end Clear Sequence

		public string summary()
		{
			string sMsg = "";
			sMsg += "           Filename: " + info.filename + LOR4Admin.CRLF + LOR4Admin.CRLF;
			sMsg += "              Lines: " + lineCount.ToString() + LOR4Admin.CRLF;
			sMsg += "   Regular Channels: " + Channels.Count.ToString() + LOR4Admin.CRLF;
			sMsg += "       RGB Channels: " + RGBchannels.Count.ToString() + LOR4Admin.CRLF;
			sMsg += "     Channel Groups: " + ChannelGroups.Count.ToString() + LOR4Admin.CRLF;
			sMsg += "       Timing Grids: " + TimingGrids.Count.ToString() + LOR4Admin.CRLF;
			sMsg += "             Tracks: " + Tracks.Count.ToString() + LOR4Admin.CRLF;
			sMsg += "       Centiseconds: " + Centiseconds.ToString() + LOR4Admin.CRLF;
			sMsg += "Highest Saved Index: " + AllMembers.HighestSavedIndex.ToString() + LOR4Admin.CRLF;
			return sMsg;
		}

		public int[] DuplicateNameCheck()
		{
			// returns null if no matches
			// if matches found, returns array with pairs of matches
			// Thus array size will always be factor of 2
			// with even numbers being a Match to the odd number just after it
			// (or odd numbers being a Match to the even number just before it)
			int[] ret = null;
			Match[] matches = null;
			int q = 0;
			// Do we even have any Channels?  If so...
			if (Channels.Count > 0)
			{
				// resize list to channel count
				Array.Resize(ref matches, Channels.Count);
				// loop thru Channels, collect Name and info
				for (int ch = 0; ch < Channels.Count; ch++)
				{
					matches[ch].Name = Channels[ch].Name;
					matches[ch].savedIdx = Channels[ch].SavedIndex;
					matches[ch].MemberType = LOR4MemberType.Channel;
					matches[ch].itemIdx = ch;
				}
				q = Channels.Count;
			} // channel count > 0

			// Any RGB Channels?
			if (RGBchannels.Count > 0)
			{
				// Loop thru 'em and add their Name and info to the list
				for (int rg = 0; rg < RGBchannels.Count; rg++)
				{
					Array.Resize(ref matches, q + 1);
					matches[q].Name = RGBchannels[rg].Name;
					matches[q].savedIdx = RGBchannels[rg].SavedIndex;
					matches[q].MemberType = LOR4MemberType.RGBChannel;
					matches[q].itemIdx = rg;

					q++;
				}
			} // RGB Channel Count > 0

			// Again for channel groups
			if (ChannelGroups.Count > 0)
			{
				for (int gr = 0; gr < ChannelGroups.Count; gr++)
				{
					Array.Resize(ref matches, q + 1);
					matches[q].Name = ChannelGroups[gr].Name;
					matches[q].savedIdx = ChannelGroups[gr].SavedIndex;
					matches[q].MemberType = LOR4MemberType.ChannelGroup;
					matches[q].itemIdx = gr;

					q++;
				}
			} // end groups

			// Do we have at least 2 names
			if (q > 1)
			{
				// Sort by Name!
				SortMatches(matches, 0, matches.Length);
				int y = 0;
				// Loop thru sorted list, comparing each member to the previous one
				for (int ql = 1; ql < q; ql++)
				{
					if (matches[ql].Name == matches[q].Name)
					{
						// If they Match, add 2 elements to the output array
						Array.Resize(ref ret, y + 2);
						// and add their saved indexes
						ret[y] = matches[ql - 1].savedIdx;
						ret[y + 1] = matches[ql].savedIdx;
						y += 2;
					}
				} // end loop thru sorted list
			} // end at least 2 names

			return ret;
		}

		private void SortMatches(Match[] matches, int low, int high)
		{
			int pivot_loc = (low + high) / 2;

			if (low < high)
				pivot_loc = PartitionMatches(matches, low, high);
			SortMatches(matches, low, pivot_loc - 1);
			SortMatches(matches, pivot_loc + 1, high);
		}

		private int PartitionMatches(Match[] matches, int low, int high)
		{
			string pivot = matches[high].Name;
			int i = low - 1;

			for (int j = low; j < high - 1; j++)
			{
				if (matches[j].Name.CompareTo(pivot) <= 0)
				{
					i++;
					SwapMatches(matches, i, j);
				}
			}
			SwapMatches(matches, i + 1, high);
			return i + 1;
		}

		private void SwapMatches(Match[] matches, int idx1, int idx2)
		{
			Match temp = matches[idx1];
			matches[idx1] = matches[idx2];
			matches[idx2] = temp;
			//MakeDirty();

		}

		public void CentiFix()
		{
			int maxCS = 180000;
			int bigCS = 0;
			// Find the largest REASONABLE (less than 30 minutes) time
			if (myCentiseconds < maxCS)
			{
				if (myCentiseconds > bigCS) bigCS = myCentiseconds;
			}
			foreach (LOR4Channel chan in Channels)
			{
				if (chan.Centiseconds < maxCS)
				{
					if (chan.Centiseconds > bigCS) bigCS = chan.Centiseconds;
				}
				foreach (LOR4Effect eff in chan.effects)
				{
					if (eff.endCentisecond < maxCS)
					{
						if (eff.endCentisecond > bigCS) bigCS = eff.endCentisecond;
					}
				}
			}
			foreach (LOR4Track trk in Tracks)
			{
				if (trk.Centiseconds < maxCS)
				{
					if (trk.Centiseconds > bigCS) bigCS = trk.Centiseconds;
				}
			}

			if (bigCS < LOR4Admin.MINCentiseconds)
			{
				string msg = "Probable Error running 'CentiFix'.  The sequence is less than 1 second long!";
				DialogResult dr = MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				CentiFix(bigCS);
			}
		}

		public void CentiFix(int newCentiseconds)
		{
			// Sets the sequence, and all tracks, channels, groups, to the new time (length)
			// And removes any timing Markers or effects after that time.
			if (newCentiseconds > LOR4Admin.MAXCentiseconds)
			{
				string m = "WARNING!! Setting Centiseconds to more than 60 minutes!  Are you sure?";
				Fyle.WriteLogEntry(m, "Warning");
				if (Fyle.DebugMode)
				{
					System.Diagnostics.Debugger.Break();
				}
			}
			if (newCentiseconds < LOR4Admin.MINCentiseconds)
			{
				string m = "WARNING!! Setting Centiseconds to less than 1 second!  Are you sure?";
				Fyle.WriteLogEntry(m, "Warning");
				if (Fyle.DebugMode)
				{
					System.Diagnostics.Debugger.Break();
				}
			}
			myCentiseconds = newCentiseconds;
			for (int i = 0; i < Tracks.Count; i++)
			{
				Tracks[i].Centiseconds = newCentiseconds;
			}
			for (int i = 0; i < TimingGrids.Count; i++)
			{
				TimingGrids[i].Centiseconds = newCentiseconds;
				if (TimingGrids[i].TimingGridType == LOR4TimingGridType.Freeform)
				{
					int t = TimingGrids[i].timings.Count - 1;
					while (t >= 0)
					{
						if (TimingGrids[i].timings[t] > newCentiseconds)
						{
							TimingGrids[i].timings.RemoveAt(t);
						}
						else
						{
							//t--;
						}
						t--;
					}
				}
			}
			for (int i = 0; i < Channels.Count; i++)
			{
				Channels[i].Centiseconds = newCentiseconds;
				if (Channels[i].effects.Count > 0)
				{
					int e = Channels[i].effects.Count - 1;
					while (e >= 0)
					{
						if (Channels[i].effects[e].startCentisecond >= newCentiseconds)
						{
							Channels[i].effects.RemoveAt(e);
						}
						else
						{
							if (Channels[i].effects[e].endCentisecond > newCentiseconds)
							{
								Channels[i].effects.RemoveAt(e);
							}
							else
							{
								//e--;
							}
						}
						e--;
					}
				}
			}
			for (int i = 0; i < RGBchannels.Count; i++)
			{
				RGBchannels[i].Centiseconds = newCentiseconds;
			}
			for (int i = 0; i < ChannelGroups.Count; i++)
			{
				ChannelGroups[i].Centiseconds = newCentiseconds;
			}
		}


		public int ReadClipboardFile(string existingFilename)
		{
			errorStatus = 0;
			reader = new StreamReader(existingFilename);
			string lineIn; // line read in (does not get modified)
			int pos1 = LOR4Admin.UNDEFINED; // positions of certain key text in the line

			// Zero these out from any previous run
			Clear(true);

			int curChannel = LOR4Admin.UNDEFINED;
			int curSavedIndex = LOR4Admin.UNDEFINED;
			int curEffect = LOR4Admin.UNDEFINED;
			int curTimingGrid = LOR4Admin.UNDEFINED;
			int curGridItem = LOR4Admin.UNDEFINED;

			// * PASS 1 - COUNT OBJECTS
			while (!reader.EndOfStream)
			{
				lineIn = reader.ReadLine();
				lineCount++;
				// does this line mark the start of a channel?
				//pos1 = lineIn.IndexOf("xml version=");
				pos1 = LOR4Admin.ContainsKey(lineIn, "xml version=");
				if (pos1 > 0)
				{
					info.xmlInfo = lineIn;
				}
				//pos1 = lineIn.IndexOf("saveFileVersion=");
				pos1 = LOR4Admin.ContainsKey(lineIn, "saveFileVersion=");
				if (pos1 > 0)
				{
					info.Parse(lineIn);
				}
				//pos1 = lineIn.IndexOf(LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.FIELDname);
				pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.FIELDname);
				if (pos1 > 0)
				{
					//channelsCount++;
				}
				//pos1 = lineIn.IndexOf(LOR4Admin.STFLD + TABLEeffect + LOR4Admin.SPC);
				pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Admin.STFLD + TABLEeffect + LOR4Admin.SPC);
				if (pos1 > 0)
				{
					//effectCount++;
				}
				if (Tracks.Count == 0)
				{
				}
				//pos1 = lineIn.IndexOf(LOR4Admin.STFLD + TABLEtimingGrid + LOR4Admin.SPC);
				pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Admin.STFLD + TABLEtimingGrid + LOR4Admin.SPC);
				if (pos1 > 0)
				{
					//timingGridCount++;
				}
				//pos1 = lineIn.IndexOf(LOR4Admin.STFLD + LOR4Timings.TABLEtiming + LOR4Admin.SPC);
				pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Admin.STFLD + LOR4Timings.TABLEtiming + LOR4Admin.SPC);
				if (pos1 > 0)
				{
					//gridItemCount++;
				}

				//pos1 = lineIn.IndexOf(LOR4Admin.FIELDsavedIndex);
				pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Admin.FIELDsavedIndex);
				if (pos1 > 0)
				{
					curSavedIndex = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
					//if (curSavedIndex > bySavedIndex.highestSavedIndex)
					{
						//Members.highestSavedIndex = curSavedIndex;
					}
				}
			}

			reader.Close();
			// CREATE ARRAYS TO HOLD OBJECTS
			//Channels = new channel[channelsCount + 2];
			//savedIndexes = new SavedIndex[highestSavedIndex + 3];
			//RGBchannels = new rgbChannel[rgbChannelCount + 2];
			//TimingGrids = new timingGrid[timingGridCount + 2];
			//Tracks = new track[1];
			//int pixNo = 0;
			int chwhich = 0;

			//////////////////////////////////
			// * PASS 2 - COLLECT OBJECTS * //
			//////////////////////////////////
			reader = new StreamReader(existingFilename);
			lineCount = 0;
			while (!reader.EndOfStream)
			{
				lineIn = reader.ReadLine();
				lineCount++;
				// have we reached the Tracks section?
				// does this line mark the start of a regular channel?
				//pos1 = lineIn.IndexOf(LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD);
				pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD);
				if (pos1 > 0)
				{
					curChannel++;
					//LOR4Channel chan = new LOR4Channel();
					//chan.Name = "ch" + curChannel.ToString("00000");
					if (chwhich == 0)
					{
						//chan.Name += "(R)";
					}
					if (chwhich == 1)
					{
						//chan.Name += "(G)";
					}
					if (chwhich == 2)
					{
						//chan.Name += "(B)";
					}

					//chan.Name += "p" + pixNo.ToString("00000");
					//chan.color = LOR4Admin.getKeyValue(lineIn, FIELDcolor);
					//chan.Centiseconds = LOR4Admin.getKeyValue(lineIn, FIELDcentiseconds);
					//chan.deviceType = LOR4SeqEnums.EnumDevice(LOR4Admin.getKeyWord(lineIn, FIELDdeviceType));
					//chan.unit = LOR4Admin.getKeyValue(lineIn, FIELDunit);
					//chan.network = LOR4Admin.getKeyValue(lineIn, FIELDnetwork);
					//chan.circuit = LOR4Admin.getKeyValue(lineIn, FIELDcircuit);
					//chan.SavedIndex = LOR4Admin.getKeyValue(lineIn, FIELDsavedIndex);
					//Channels[curChannel] = chan;
					//curSavedIndex = chan.SavedIndex;

					//si.PartType = LOR4MemberType.channel;
					//si.objIndex = curChannel;
					//savedIndexes[curSavedIndex] = si;
					if (chwhich == 2)
					{ }
					chwhich++;
					chwhich %= 3;

				}

				// does this line mark the start of an Effect?
				//pos1 = lineIn.IndexOf(TABLEeffect + LOR4Admin.FIELDtype);
				pos1 = LOR4Admin.ContainsKey(lineIn, TABLEeffect + LOR4Admin.FIELDtype);
				if (pos1 > 0)
				{
					curEffect++;

					//DEBUG!
					if (curEffect > 638)
					{
						errorStatus = 1;
					}

					LOR4Effect ef = new LOR4Effect();
					ef.EffectType = LOR4SeqEnums.EnumEffectType(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDtype));
					ef.startCentisecond = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDstartCentisecond);
					ef.endCentisecond = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDendCentisecond);
					ef.Intensity = LOR4Admin.getKeyValue(lineIn, LOR4Admin.SPC + LOR4Effect.FIELDintensity);
					ef.startIntensity = LOR4Admin.getKeyValue(lineIn, LOR4Effect.FIELDstartIntensity);
					ef.endIntensity = LOR4Admin.getKeyValue(lineIn, LOR4Effect.FIELDendIntensity);
					Channels[curChannel].effects.Add(ef);
				}

				// does this line mark the start of a Timing Grid?
				//pos1 = lineIn.IndexOf(LOR4Admin.STFLD + TABLEtimingGrid + LOR4Admin.SPC);
				pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Admin.STFLD + TABLEtimingGrid + LOR4Admin.SPC);
				if (pos1 > 0)
				{
					curTimingGrid++;
					//LOR4Timings tg = new LOR4Timings();
					//tg.Name = LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname);
					//tg.type = LOR4SeqEnums.EnumGridType(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDtype));
					//tg.SavedIndex = LOR4Admin.getKeyValue(lineIn, LOR4Timings.FIELDsaveID);
					//tg.spacing = LOR4Admin.getKeyValue(lineIn, LOR4Timings.FIELDspacing);
					//TimingGrids[curTimingGrid] = tg;

					//if (tg.type == LOR4TimingGridType.Freeform)
					{
						lineIn = reader.ReadLine();
						lineCount++;
						//pos1 = lineIn.IndexOf(LOR4Timings.TABLEtiming + LOR4Admin.FIELDcentisecond);
						pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Timings.TABLEtiming + LOR4Admin.FIELDcentisecond);
						while (pos1 > 0)
						{
							curGridItem++;
							int gpos = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDcentisecond);
							TimingGrids[curTimingGrid].AddTiming(gpos);

							lineIn = reader.ReadLine();
							lineCount++;
							//pos1 = lineIn.IndexOf(LOR4Timings.TABLEtiming + LOR4Admin.FIELDcentisecond);
							pos1 = LOR4Admin.ContainsKey(lineIn, LOR4Timings.TABLEtiming + LOR4Admin.FIELDcentisecond);
						}
					} // end grid is freeform
				} // end if timingGrid
			} // end while line is valid

			reader.Close();
			Centiseconds = Tracks[0].Centiseconds;

			if (errorStatus <= 0)
			{
				info.filename = existingFilename;
			}

			return errorStatus;
		}

		private int RenameTempFile(string finalFilename)
		{

			if (errorStatus == 0)
			{
				if (File.Exists(finalFilename))
				{
					//string bakFile = newFilename.Substring(0, newFilename.Length - 3) + "bak";
					string bakFile = finalFilename + Fyle.EXT_BAK;
					if (File.Exists(bakFile))
					{
						File.Delete(bakFile);
					}
					File.Move(finalFilename, bakFile);
				}
				File.Move(tempFileName, finalFilename);

				if (errorStatus <= 0)
				{
					//info.filename = finalFilename;
				}
			}

			return errorStatus;

		}

		public int WriteClipboardFile(string newFilename)
		{
			//TODO: This procedure is totally untested!!

			errorStatus = 0;
			lineCount = 0;

			//backupFile(fileName);

			string tmpFile = newFilename + Fyle.EXT_TMP;

			writer = new StreamWriter(tmpFile);
			lineOut = ""; // line to be Written out, gets modified if necessary
										//int pos1 = LOR4Admin.UNDEFINED; // positions of certain key text in the line

			int curTimingGrid = 0;
			//int curGridItem = 0;
			//int curTrack = 0;
			//int curTrackItem = 0;
			int[] destSIs = new int[1];
			//int destSI = LOR4Admin.UNDEFINED;
			updatedTrack[] updatedTracks = new updatedTrack[Tracks.Count];

			lineOut = info.xmlInfo;
			writer.WriteLine(lineOut);
			lineOut = LOR4Admin.STFLD + TABLEchannelsClipboard + " version=\"1\" Name=\"" + Path.GetFileNameWithoutExtension(newFilename) + "\"" + LOR4Admin.ENDFLD;
			writer.WriteLine(lineOut);

			// Write Timing Grid aka cellDemarcation
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.STFLD + TABLEcellDemarcation + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);
			for (int tm = 0; tm < TimingGrids[0].timings.Count; tm++)
			{
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.STFLD + TABLEcellDemarcation;
				lineOut += LOR4Admin.FIELDcentisecond + LOR4Admin.FIELDEQ + TimingGrids[curTimingGrid].timings[tm].ToString() + LOR4Admin.ENDQT;
				lineOut += LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);
			}
			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.FINTBL + TABLEcellDemarcation + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);

			// Write JUST CHANNELS in display order
			// DO NOT write track, RGB group, or channel group info
			for (int trk = 0; trk < Tracks.Count; trk++)
			{
				for (int ti = 0; ti < Tracks[trk].Members.Count; ti++)
				{
					int si = Tracks[trk].Members.Items[ti].ID;
					ParseItemsToClipboard(si);
				} // end for track items loop
			} // end for Tracks loop

			lineOut = LOR4Admin.LEVEL1 + LOR4Admin.FINTBL + LOR4Admin.TABLEchannel + LOR4Admin.PLURAL + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);

			lineOut = LOR4Admin.FINTBL + TABLEchannelsClipboard + LOR4Admin.FINFLD;
			writer.WriteLine(lineOut);

			Console.WriteLine(lineCount.ToString() + " Out:" + lineOut);
			Console.WriteLine("");

			writer.Flush();
			writer.Close();

			if (File.Exists(newFilename))
			{
				string bakFile = newFilename.Substring(0, newFilename.Length - 3) + "bak";
				if (File.Exists(bakFile))
				{
					File.Delete(bakFile);
				}
				File.Move(newFilename, bakFile);
			}
			File.Move(tmpFile, newFilename);

			if (errorStatus <= 0)
			{
				info.filename = newFilename;
			}

			return errorStatus;
		} // end WriteClipboardFile

		private void ParseItemsToClipboard(int saveID)
		{
			int oi = 0; //savedIndexes[saveID].objIndex;
			LOR4MemberType itemType = LOR4MemberType.None; //savedIndexes[saveID].PartType;
			if (itemType == LOR4MemberType.Channel)
			{
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);
				//WriteEffects(Channels[oi]);
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.FINTBL + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);
			} // end if channel

			if (itemType == LOR4MemberType.RGBChannel)
			{
				LOR4RGBChannel rgbch = RGBchannels[oi];
				// Get and write Red Channel
				//int ci = RGBchannels[oi].redChannelObjIndex;
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);

				//Channels[ci].LineOut()
				//lineOut = LOR4Admin.LEVEL2 + LOR4Admin.FINTBL + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);

				// Get and write Green Channel
				//ci = RGBchannels[oi].grnChannelObjIndex;
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);
				//writeEffects(Channels[ci]);
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.FINTBL + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);

				// Get and write Blue Channel
				//ci = RGBchannels[oi].bluChannelObjIndex;
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);
				//writeEffects(Channels[ci]);
				lineOut = LOR4Admin.LEVEL2 + LOR4Admin.FINTBL + LOR4Admin.TABLEchannel + LOR4Admin.ENDFLD;
				writer.WriteLine(lineOut);
			} // end if rgbChannel

			if (itemType == LOR4MemberType.ChannelGroup)
			{
				LOR4ChannelGroup grp = ChannelGroups[oi];
				//for (int itm = 0; itm < grp.itemSavedIndexes.Count; itm++)
				//{
				//	ParseItemsToClipboard(grp.itemSavedIndexes[itm]);
				//}
			} // end if channelGroup
		} // end ParseChannelGroupToClipboard

		private class updatedTrack
		{
			public List<int> newSavedIndexes = new List<int>();
		}

		#region IdentityFindWrappers
		// Wrappers for Members Find Functions

		public LOR4Channel FindChannel(int SavedIndex)
		{
			LOR4Channel ret = null;
			iLOR4Member member = AllMembers.BySavedIndex[SavedIndex];
			if (member != null)
			{
				if (member.MemberType == LOR4MemberType.Channel)
				{
					ret = (LOR4Channel)member;
				}
			}
			return ret;
		} // end FindChannel

		public LOR4Channel FindChannel(string channelName, bool createIfNotFound = false, bool clearEffects = false)
		{
			LOR4Channel ret = null;
			iLOR4Member member = AllMembers.FindByName(channelName, LOR4MemberType.Channel, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4Channel)member;
				if (clearEffects)
				{
					ret.effects.Clear();
				}
			}
			return ret;
		}

		public LOR4Channel DEPRECIATED_FindChannel(string channelName, LOR4Membership memberList, bool clearEffects = false, bool createIfNotFound = false)
		{
			// Gets existing channel specified by Name if it already exists in the group
			// Creates it if it does not
			LOR4Channel ret = null;
			iLOR4Member part = null;
			int gidx = 0;
			while ((ret == null) && (gidx < memberList.Count))
			{
				part = memberList.Items[gidx];
				if (part.MemberType == LOR4MemberType.Channel)
				{
					if (part.Name == channelName)
					{
						ret = (LOR4Channel)part;
						// Clear any existing effects from a previous run
						if (clearEffects)
						{
							if (ret.effects.Count > 0)
							{
								ret.effects = new List<LOR4Effect>();
							}
						}
						gidx = memberList.Count; // Loopus Interruptus
					}
				}
				gidx++;
			}

			if (ret == null)
			{
				if (createIfNotFound)
				{
					//int si = Sequence.Members.HighestSavedIndex + 1;
					ret = CreateNewChannel(channelName);
					ret.Centiseconds = myCentiseconds;
					//Channels.Add(ret);
					memberList.Add(ret);
				}
			}

			return ret;
		}



		public LOR4RGBChannel FindRGBChannel(int SavedIndex)
		{
			LOR4RGBChannel ret = null;
			iLOR4Member member = AllMembers.BySavedIndex[SavedIndex];
			if (member != null)
			{
				if (member.MemberType == LOR4MemberType.RGBChannel)
				{
					ret = (LOR4RGBChannel)member;
				}
			}
			return ret;
		} // end FindrgbChannel

		public LOR4RGBChannel FindRGBChannel(string rgbChannelName, bool createIfNotFound = false, bool clearEffects = false)
		{
			LOR4RGBChannel ret = null;
			iLOR4Member member = AllMembers.FindByName(rgbChannelName, LOR4MemberType.RGBChannel, createIfNotFound);
			if (member != null)
			{
				if (member.MemberType == LOR4MemberType.RGBChannel)
				{
					ret = (LOR4RGBChannel)member;
					if (clearEffects)
					{
						ret.redChannel.effects.Clear();
						ret.grnChannel.effects.Clear();
						ret.redChannel.effects.Clear();
					}
				}
			}

			return ret;
		}

		public LOR4RGBChannel DEPRECIATED_FindRGBChannel(string rgbChannelName, LOR4Membership memberList, bool clearEffects = false, bool createIfNotFound = false)
		{
			// Gets existing channel specified by Name if it already exists in the group
			// Creates it if it does not
			LOR4RGBChannel ret = null;
			iLOR4Member part = null;
			int gidx = 0;
			while ((ret == null) && (gidx < memberList.Count))
			{
				part = memberList.Items[gidx];
				if (part.MemberType == LOR4MemberType.RGBChannel)
				{
					if (part.Name == rgbChannelName)
					{
						ret = (LOR4RGBChannel)part;
						// Clear any existing effects from a previous run
						if (clearEffects)
						{
							ret.redChannel.effects.Clear();
							ret.grnChannel.effects.Clear();
							ret.redChannel.effects.Clear();
						}
						gidx = memberList.Count; // Force exit of loop
					}
				}
				gidx++;
			}

			if (ret == null)
			{
				if (createIfNotFound)
				{
					LOR4Channel redCh = FindChannel(rgbChannelName + " (R)", true);
					redCh.color = LOR4Admin.LORCOLOR_RED;
					LOR4Channel grnCh = FindChannel(rgbChannelName + " (G)", true);
					grnCh.color = LOR4Admin.LORCOLOR_GRN;
					LOR4Channel bluCh = FindChannel(rgbChannelName + " (B)", true);
					bluCh.color = LOR4Admin.LORCOLOR_BLU;
					if (clearEffects)
					{
						redCh.effects.Clear();
						grnCh.effects.Clear();
						redCh.effects.Clear();
					}
					ret = CreateNewRGBChannel(rgbChannelName);
					ret.redChannel = redCh;
					ret.grnChannel = grnCh;
					ret.bluChannel = bluCh;
					redCh.rgbChild = LOR4RGBChild.Red;
					grnCh.rgbChild = LOR4RGBChild.Green;
					bluCh.rgbChild = LOR4RGBChild.Blue;
					redCh.rgbParent = ret;
					grnCh.rgbParent = ret;
					bluCh.rgbParent = ret;
					//int si = Sequence.Members.HighestSavedIndex + 1;
					ret.Centiseconds = myCentiseconds;
					//Channels.Add(ret);
					memberList.Add(ret);
				}
			}

			return ret;
		}

		public LOR4ChannelGroup FindChannelGroup(int SavedIndex)
		{
			LOR4ChannelGroup ret = null;
			iLOR4Member member = AllMembers.BySavedIndex[SavedIndex];
			if (member != null)
			{
				if (member.MemberType == LOR4MemberType.ChannelGroup)
				{
					ret = (LOR4ChannelGroup)member;
				}
			}
			return ret;
		} // end FindChannelGroup

		public LOR4ChannelGroup DEPRECIATED_FindChannelGroup(string channelGroupName, LOR4Membership memberList, bool createIfNotFound = false)
		{
			// Gets existing group specified by Name if it already exists in the track or group
			// Creates it if it does not
			// Can't use 'Find' functions because we only want to look in this one particular track or group

			// Create blank/null return object
			LOR4ChannelGroup ret = null;

			int gidx = 0; // loop counter
										// loop while we still have no group, and we haven't reached to end of the list
			while ((ret == null) && (gidx < memberList.Count))
			{
				// Get each item's ID
				//int SI = Children.Items[gidx].SavedIndex;
				iLOR4Member part = memberList.Items[gidx];
				if (part.MemberType == LOR4MemberType.ChannelGroup)
				{
					LOR4ChannelGroup group = (LOR4ChannelGroup)part;
					if (group.Name == channelGroupName)
					{
						// Found it!!
						ret = group;
						gidx = memberList.Count;  // Force exit of loop
					}
				}
				gidx++;
			}


			if (ret == null)
			{
				// Not found, create it?
				if (createIfNotFound)
				{
					ret = CreateNewChannelGroup(channelGroupName);
					ret.Centiseconds = myCentiseconds;
					//ChannelGroups.Add(ret);
					memberList.Add(ret);
				}
			}

			return ret;

		}


		public LOR4ChannelGroup FindChannelGroup(string channelGroupName, bool createIfNotFound = false)
		{
			LOR4ChannelGroup ret = null;
			iLOR4Member member = AllMembers.FindByName(channelGroupName, LOR4MemberType.ChannelGroup, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4ChannelGroup)member;
			}
			return ret;
		}

		/*
		public LOR4Track OLD_FindTrack(string trackName, bool createIfNotFound = false)
		{
			LOR4Track ret = null;
			// This no longer works because Tracks are no longer in AllMembers because they don't have a SavedIndex
			//iLOR4Member member = AllMembers.FindByName(trackName, MemberType.Track, createIfNotFound);
			iLOR4Member member = Members.FindByName(trackName, LOR4MemberType.Track, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4Track)member;
			}
			return ret;
		}
		*/

		// LOR4Sequence.FindTimingGrid(name, create)
		public LOR4Timings FindTimingGrid(string timingGridName, bool createIfNotFound = false)
		{
#if DEBUG
			string msg = "Sequence.FindTimingGrid(" + timingGridName + ", " + createIfNotFound.ToString() + ")";
			Debug.WriteLine(msg);
#endif
			LOR4Timings ret = null;
			//iLOR4Member member = Members.Find(timingGridName, LOR4MemberType.Timings, createIfNotFound);
			//iLOR4Member member = null;
			for (int i = 0; i < TimingGrids.Count; i++)
			{
				LOR4Timings member = TimingGrids[i];
				if (member.Name == timingGridName)
				{
					ret = member;
					i = TimingGrids.Count; // exit for loop
				}
			}
			if (ret == null)
			{
				if (createIfNotFound)
				{
					ret = CreateNewTimingGrid(timingGridName);
					//TimingGrids.Add(ret);
				}
			}
			return ret;
		}
		#endregion

		public int SavedIndexIntegrityCheck()
		{
			// Returns 0 if no problems found
			// number > 0 = number of problems
			int errs = 0;



			int tot = Channels.Count + RGBchannels.Count + ChannelGroups.Count;
			LOR4SavedIndex[] siCheck = null;
			Array.Resize(ref siCheck, tot);
			for (int t = 0; t < tot; t++)
			{
				siCheck[t] = new LOR4SavedIndex();
			}

			for (int ch = 0; ch < Channels.Count; ch++)
			{
				if (Channels[ch].SavedIndex < tot)
				{
					if (siCheck[Channels[ch].SavedIndex].objIndex == LOR4Admin.UNDEFINED)
					{
						siCheck[Channels[ch].SavedIndex].objIndex = ch;
					}
					else
					{
						errs++;
					}
					if (siCheck[Channels[ch].SavedIndex].MemberType == LOR4MemberType.None)
					{
						siCheck[Channels[ch].SavedIndex].MemberType = LOR4MemberType.Channel;
					}
					else
					{
						errs++;
					}
				}
				else
				{
					errs++;
				}
			} // end Channels loop

			for (int rch = 0; rch < RGBchannels.Count; rch++)
			{
				if (RGBchannels[rch].SavedIndex < tot)
				{
					if (siCheck[RGBchannels[rch].SavedIndex].objIndex == LOR4Admin.UNDEFINED)
					{
						siCheck[RGBchannels[rch].SavedIndex].objIndex = rch;
					}
					else
					{
						errs++;
					}
					if (siCheck[RGBchannels[rch].SavedIndex].MemberType == LOR4MemberType.None)
					{
						siCheck[RGBchannels[rch].SavedIndex].MemberType = LOR4MemberType.RGBChannel;
					}
					else
					{
						errs++;
					}
				}
				else
				{
					errs++;
				}
			} // end RGBchannels loop

			for (int chg = 0; chg < ChannelGroups.Count; chg++)
			{
				if (ChannelGroups[chg].SavedIndex < tot)
				{
					if (siCheck[ChannelGroups[chg].SavedIndex].objIndex == LOR4Admin.UNDEFINED)
					{
						siCheck[ChannelGroups[chg].SavedIndex].objIndex = chg;
					}
					else
					{
						errs++;
					}
					if (siCheck[ChannelGroups[chg].SavedIndex].MemberType == LOR4MemberType.None)
					{
						siCheck[ChannelGroups[chg].SavedIndex].MemberType = LOR4MemberType.ChannelGroup;
					}
					else
					{
						errs++;
					}
				}
				else
				{
					errs++;
				}
			} // end Channels loop

			if (tot != AllMembers.HighestSavedIndex + 1)
			{
				errs++;
			}



			return errs;
		}

		public int[] OutputConflicts()
		{
			int[] ret = null;  // holds pairs of matches with identical outputs
			string[] outputs = null;  // holds outputs of each channel in string format (that are not controllerType None)
			int[] indexes = null; // holds SavedIndex of Channels
			int outputCount = 0; // how many Channels (so far) are not controllerType None
			int matches = 0; // how many matches (X2) have been found (so far)  NOTE: matches are returned in pairs
											 //                                                                   So ret[even#] matches ret[odd#]
											 // Loop thru all regular Channels
			for (int ch = 0; ch < Channels.Count; ch++)
			{
				// if deviceType != None
				if (Channels[ch].output.deviceType != LOR4DeviceType.None)
				{
					// store output info in string format
					Array.Resize(ref outputs, outputCount + 1);
					outputs[outputCount] = Channels[ch].output.ToString();
					// store the SavedIndex of the channel
					Array.Resize(ref indexes, outputCount + 1);
					indexes[outputCount] = Channels[ch].SavedIndex;
					// incremnt count
					outputCount++;
				}
			} // end channel loop
				// if at least 2 Channels deviceType != None
			if (outputCount > 1)
			{
				// Sort the output info (in string format) along with the savedIndexes
				Array.Sort(outputs, indexes);
				// loop thru sorted arrays
				for (int q = 1; q < outputCount; q++)
				{
					// compare output info
					if (outputs[q - 1] == outputs[q])
					{
						// if Match, store savedIndexes in pair of return values
						Array.Resize(ref ret, matches + 2);
						ret[matches] = indexes[q - 1];
						ret[matches + 1] = indexes[q];
						// increment return value count by one pair
						matches += 2;
					}
				} // end output loop
			} // end if outputCount > 1
				// return any matches found
			return ret;
		} // end output conflicts

		public int[] DuplicateChannelNames()
		{
			int[] ret = null;  // holds pairs of matches with identical names
			string[] names = null;  // holds Name of each channel
			int[] indexes = null; // holds SavedIndex of Channels
			int matches = 0; // how many matches (X2) have been found (so far)  NOTE: matches are returned in pairs
											 //                                                                   So ret[even#] matches ret[odd#]

			if (Channels.Count > 1)
			{
				Array.Resize(ref names, Channels.Count);
				// Loop thru all regular Channels
				for (int ch = 0; ch < Channels.Count; ch++)
				{
					names[ch] = Channels[ch].Name;
					indexes[ch] = Channels[ch].SavedIndex;
				} // end channel loop
					// Sort the output info (in string format) along with the savedIndexes
				Array.Sort(names, indexes);
				// loop thru sorted arrays
				for (int q = 1; q < Channels.Count; q++)
				{
					// compare output info
					if (names[q - 1] == names[q])
					{
						// if Match, store savedIndexes in pair of return values
						Array.Resize(ref ret, matches + 2);
						ret[matches] = indexes[q - 1];
						ret[matches + 1] = indexes[q];
						// increment return value count by one pair
						matches += 2;
					}
				} // end output loop
			} // end if outputCount > 1
				// return any matches found
			return ret;
		} // end DuplicateChannelNames function

		public int[] DuplicateChannelGroupNames()
		{
			int[] ret = null;  // holds pairs of matches with identical names
			string[] names = null;  // holds Name of each channelGroup
			int[] indexes = null; // holds SavedIndex of ChannelGroups
			int matches = 0; // how many matches (X2) have been found (so far)  NOTE: matches are returned in pairs
											 //                                                                   So ret[even#] matches ret[odd#]

			if (ChannelGroups.Count > 1)
			{
				Array.Resize(ref names, ChannelGroups.Count);
				// Loop thru all regular Channels
				for (int chg = 0; chg < ChannelGroups.Count; chg++)
				{
					names[chg] = ChannelGroups[chg].Name;
					indexes[chg] = ChannelGroups[chg].SavedIndex;
				} // end channelGroup loop
					// Sort the output info (in string format) along with the savedIndexes
				Array.Sort(names, indexes);
				// loop thru sorted arrays
				for (int q = 1; q < ChannelGroups.Count; q++)
				{
					// compare output info
					if (names[q - 1] == names[q])
					{
						// if Match, store savedIndexes in pair of return values
						Array.Resize(ref ret, matches + 2);
						ret[matches] = indexes[q - 1];
						ret[matches + 1] = indexes[q];
						// increment return value count by one pair
						matches += 2;
					}
				} // end output loop
			} // end if outputCount > 1
				// return any matches found
			return ret;
		} // end Duplicate Channel Group Names

		public int[] DuplicateRGBchannelNames()
		{
			int[] ret = null;  // holds pairs of matches with identical names
			string[] names = null;  // holds Name of each rgbChannel
			int[] indexes = null; // holds SavedIndex of RGBchannels
			int matches = 0; // how many matches (X2) have been found (so far)  NOTE: matches are returned in pairs
											 //                                                                   So ret[even#] matches ret[odd#]

			if (RGBchannels.Count > 1)
			{
				Array.Resize(ref names, RGBchannels.Count);
				// Loop thru all regular Channels
				for (int rch = 0; rch < RGBchannels.Count; rch++)
				{
					names[rch] = RGBchannels[rch].Name;
					indexes[rch] = RGBchannels[rch].SavedIndex;
				} // end channel loop
					// if at least 2 Channels deviceType != None
					// Sort the output info (in string format) along with the savedIndexes
				Array.Sort(names, indexes);
				// loop thru sorted arrays
				for (int q = 1; q < RGBchannels.Count; q++)
				{
					// compare output info
					if (names[q - 1] == names[q])
					{
						// if Match, store savedIndexes in pair of return values
						Array.Resize(ref ret, matches + 2);
						ret[matches] = indexes[q - 1];
						ret[matches + 1] = indexes[q];
						// increment return value count by one pair
						matches += 2;
					}
				} // end output loop
			} // end if outputCount > 1
				// return any matches found
			return ret;
		} // end Duplicate RGB Channel Names

		public int[] DuplicateTrackNames()
		{
			int[] ret = null;  // holds pairs of matches with identical names
			string[] names = null;  // holds Name of each track
			int[] indexes = null; // holds ObjectIndex of track
			int matches = 0; // how many matches (X2) have been found (so far)  NOTE: matches are returned in pairs
											 //                                                                   So ret[even#] matches ret[odd#]

			if (Tracks.Count > 1)
			{
				Array.Resize(ref names, Tracks.Count);
				// Loop thru all regular Channels
				for (int tr = 0; tr < Tracks.Count; tr++)
				{
					names[tr] = Tracks[tr].Name;
					indexes[tr] = tr;
				} // end channel loop
					// if at least 2 Channels deviceType != None
					// Sort the output info (in string format) along with the savedIndexes
				Array.Sort(names, indexes);
				// loop thru sorted arrays
				for (int q = 1; q < Tracks.Count; q++)
				{
					// compare output info
					if (names[q - 1] == names[q])
					{
						// if Match, store savedIndexes in pair of return values
						Array.Resize(ref ret, matches + 2);
						ret[matches] = indexes[q - 1];
						ret[matches + 1] = indexes[q];
						// increment return value count by one pair
						matches += 2;
					}
				} // end output loop
			} // end if outputCount > 1
				// return any matches found
			return ret;
		} // end DuplicateTrackNames function

		public int[] DuplicateNames()
		{
			// Looks for duplicate names amongst regular Channels, RGB Channels, channel groups, and Tracks
			// Does not include timing grid names
			int[] ret = null;  // holds pairs of matches with identical names
			string[] names = null;  // holds Name of each channel
			int nameCount = 0;
			int[] indexes = null; // holds SavedIndex of Channels
			int matches = 0; // how many matches (X2) have been found (so far)  NOTE: matches are returned in pairs
											 //                                                                   So ret[even#] matches ret[odd#]

			nameCount = Channels.Count + RGBchannels.Count + ChannelGroups.Count + Tracks.Count;
			if (nameCount > 1)
			{
				Array.Resize(ref names, nameCount);
				// Loop thru all regular Channels
				for (int ch = 0; ch < Channels.Count; ch++)
				{
					names[ch] = Channels[ch].Name;
					indexes[ch] = Channels[ch].SavedIndex;
				} // end channel loop

				for (int rch = 0; rch < RGBchannels.Count; rch++)
				{
					names[rch + Channels.Count] = RGBchannels[rch].Name;
					indexes[rch + Channels.Count] = RGBchannels[rch].SavedIndex;
				} // end RGB channel loop

				for (int chg = 0; chg < ChannelGroups.Count; chg++)
				{
					names[chg + Channels.Count + RGBchannels.Count] = ChannelGroups[chg].Name;
					indexes[chg + Channels.Count + RGBchannels.Count] = ChannelGroups[chg].SavedIndex;
				} // end Channel Group Loop

				int trIdx;
				for (int tr = 0; tr < Tracks.Count; tr++)
				{
					names[tr + Channels.Count + RGBchannels.Count + ChannelGroups.Count] = Tracks[tr].Name;
					// use negative numbers for track indexes
					trIdx = LOR4Admin.UNDEFINED + (-tr);
					indexes[tr + Channels.Count + RGBchannels.Count + ChannelGroups.Count] = trIdx;
				}

				// Sort the output info (in string format) along with the savedIndexes
				Array.Sort(names, indexes);
				// loop thru sorted arrays
				for (int q = 1; q < Channels.Count; q++)
				{
					// compare output info
					if (names[q - 1] == names[q])
					{
						// if Match, store savedIndexes in pair of return values
						Array.Resize(ref ret, matches + 2);
						ret[matches] = indexes[q - 1];
						ret[matches + 1] = indexes[q];
						// increment return value count by one pair
						matches += 2;
					}
				} // end output loop
			} // end if nameCount > 1
				// return any matches found
			return ret;
		} // end DuplicateNames function

		private int BTreeFindName(string[] names, string Name)
		{
			// looks for an EXACT Match (see also: FuzzyFindName)
			// names[] must be sorted!

			//TODO Test this THOROUGHLY!

			int ret = LOR4Admin.UNDEFINED;
			int bot = 0;
			int top = Tracks.Count - 1;
			int mid = ((top - bot) / 2) + bot;

			while (top > bot)
			{
				mid = ((top - bot) / 2) + bot;

				if (names[mid].CompareTo(Name) < 0)
				{
					mid = top;
				}
				else
				{
					if (names[mid].CompareTo(Name) > 0)
					{
						mid = bot;
					}
					else
					{
						return mid;
					}
				}
			}
			return ret;
		} // end bTreeFindName

		public void ClearAllEffects()
		{
			foreach (LOR4Channel ch in Channels)
			{
				ch.effects = new List<LOR4Effect>();
			}
			MakeDirty(true);
		}

		public int CopyEffects(LOR4Channel SourceChan, LOR4Channel DestChan, bool Merge)
		{
			// Could be a static method, but easier to work with if not
			int copiedCount = 0;
			if (!Merge)
			{
				DestChan.effects = new List<LOR4Effect>();
			}
			copiedCount = DestChan.CopyEffects(SourceChan.effects, Merge);
			MakeDirty(true);

			return copiedCount;
		}

		public LOR4Channel CreateNewChannel(string lineIn)
		{
			// Does NOT check to see if a channel with this name already exists
			// Therefore, allows for duplicate channel names (But they will have different SavedIndexes)
			LOR4Channel chan = new LOR4Channel(this, lineIn);
			int newSI = AssignNextSavedIndex(chan);
			chan.SetIndex(Channels.Count);
			Channels.Add(chan);
			AllMembers.Add(chan);
			myCentiseconds = Math.Max(myCentiseconds, chan.Centiseconds);
			return chan;
		}

		public LOR4RGBChannel CreateNewRGBChannel(string lineIn, bool andChildren = false)
		{
			// Does NOT check to see if a rgb channel with this name already exists
			// Therefore, allows for duplicate rgb channel names (But they will have different SavedIndexes)
			//! Important: This does NOT create the 3 child regular channels for Red, Green, and Blue.

			LOR4RGBChannel rch = new LOR4RGBChannel(this, lineIn);
			int newSI = AssignNextSavedIndex(rch);
			rch.SetIndex(RGBchannels.Count);
			RGBchannels.Add(rch);
			AllMembers.Add(rch);
			myCentiseconds = Math.Max(myCentiseconds, rch.Centiseconds);
			if (andChildren)
			{
				LOR4Channel ch = CreateNewChannel(rch.Name + " (R)");
				ch.rgbChild = LOR4RGBChild.Red;
				ch.color = LOR4Admin.LORCOLOR_RED;
				rch.redChannel = ch;
				ch = CreateNewChannel(rch.Name + " (G)");
				ch.rgbChild = LOR4RGBChild.Green;
				ch.color = LOR4Admin.LORCOLOR_GRN;
				rch.grnChannel = ch;
				ch = CreateNewChannel(rch.Name + " (B)");
				ch.rgbChild = LOR4RGBChild.Blue;
				ch.color = LOR4Admin.LORCOLOR_BLU;
				rch.bluChannel = ch;
			}
			return rch;
		}

		public LOR4ChannelGroup CreateNewChannelGroup(string lineIn)
		{
			LOR4ChannelGroup chg = new LOR4ChannelGroup(this, lineIn);
			int newSI = AssignNextSavedIndex(chg);
			chg.SetIndex(ChannelGroups.Count);
			ChannelGroups.Add(chg);
			AllMembers.Add(chg);
			myCentiseconds = Math.Max(myCentiseconds, chg.Centiseconds);
			return chg;
		}

		public LOR4Cosmic CreateNewCosmicDevice(string lineIn)
		{
			LOR4Cosmic cos = new LOR4Cosmic(this, lineIn);
			int newSI = AssignNextSavedIndex(cos);
			cos.SetIndex(CosmicDevices.Count);
			CosmicDevices.Add(cos);
			AllMembers.Add(cos);
			myCentiseconds = Math.Max(myCentiseconds, cos.Centiseconds);
			return cos;
		}

		public LOR4Track CreateNewTrack(string lineIn)
		{
			LOR4Track tr = new LOR4Track(this, lineIn);
			//if (Tracks.Count == 0) Tracks.Add(null);
			tr.SetIndex(Tracks.Count);
			tr.SetID(Tracks.Count);
			//tr.SetTrackNumber(Tracks.Count + 1);
			Tracks.Add(tr);
			if ((tr.AltID >= 0) && (tr.AltID < TimingGrids.Count))
			{
				tr.timingGrid = TimingGrids[tr.AltID];
			}
			else
			{
				if (TimingGrids.Count < 1)
				{
					LOR4Timings tempGrid = CreateNewTimingGrid("Fixed 0.10");
					tempGrid.TimingGridType = LOR4TimingGridType.FixedGrid;
					tempGrid.spacing = 10;
				}
				tr.timingGrid = TimingGrids[0];
			}
			// Clear the AltSavedIndex which was temporarily holding the SaveID of the Timings
			tr.AltID = LOR4Admin.UNDEFINED;
			myCentiseconds = Math.Max(myCentiseconds, tr.Centiseconds);
			//? AllMembers.Add(tr);
			Members.Add(tr);
			return tr;
		}
		
		public LOR4Track	NEWFindTrack(string trackName, bool createIfNotFound = false)
		{
			LOR4Track ret = null;
			int ti = LOR4Admin.UNDEFINED;
			string tName = trackName.ToLower();
			for(int tr = 0; tr < Tracks.Count; tr++)
			{
				if (Tracks[tr].Name.ToLower() == tName)
				{
					ti = tr;
					ret = Tracks[tr];
					tr = Tracks.Count; // Force exit of loop
				}
			}
			if (ret == null)
			{
				if (createIfNotFound)
				{
					ret = CreateNewTrack(trackName);
				}
			}
			return ret;
		}
		
		public LOR4Timings CreateNewTimingGrid(string lineIn)
		{
			LOR4Timings tg = new LOR4Timings(this, lineIn);
			int newSI = AssignNextSaveID(tg);
			//tg.SaveID = newSI;
			//tg.SetIndex(TimingGrids.Count);
			TimingGrids.Add(tg);
			//tg.Parse(lineIn);
			myCentiseconds = Math.Max(myCentiseconds, tg.Centiseconds);
			//Members.Add(tg);
			return tg;
			MakeDirty();
		}

		public LOR4Timings FindGrid(string gridName, bool createIfNotFound = false)
		{
			LOR4Timings ret = null;
			int gi = LOR4Admin.UNDEFINED;
			string gName = gridName.ToLower();
			for (int gr=0; gr< TimingGrids.Count; gr++)
			{
				if (TimingGrids[gr].Name.ToLower() == gName)
				{
					gi = gr;
					ret = TimingGrids[gr];
					gr = TimingGrids.Count; // Force exit of loop
				}
			}
			if (ret == null)
			{
				if (createIfNotFound)
				{
					ret = CreateNewTimingGrid(gridName);
				}
			}
			return ret;

		}
		// Sequence.CreateTimingGrid(name)
		public override void MakeDirty(bool dirtyState = true)
		{
			if (dirtyState)
			{
				info.lastModified = DateTime.Now; //.ToString("MM/dd/yyyy hh:mm:ss tt");
			}
			isDirty = dirtyState;
		}

		private int AssignNextSavedIndex(iLOR4Member thePart)
		{
			if (thePart.ID < 0)
			{
				int newSI = AllMembers.HighestSavedIndex + 1;
				thePart.SetID(newSI);
				//AllMembers.Add(thePart);
			}
			return thePart.ID;
		}

		private int AssignNextAltSavedIndex(iLOR4Member thePart)
		{
			if (thePart.AltID < 0)
			{
				int newASI = AllMembers.HighestAltSavedIndex + 1;
				thePart.AltID = newASI;
				// May cause out of range exception, might need to add instead
				while (AllMembers.ByAltSavedIndex.Count <= newASI)
				{
					AllMembers.ByAltSavedIndex.Add(null);
				}
				AllMembers.ByAltSavedIndex[thePart.AltID] = thePart;
				AllMembers.HighestAltSavedIndex = newASI;
			}
			return thePart.AltID;
		}

		private int AssignNextSaveID(LOR4Timings theGrid)
		{
			if (theGrid.SaveID < 0)
			{
				int newSI = TimingGrids.Count;
				theGrid.SetSaveID(newSI);
				//AllMembers.Add(theGrid);
			}
			return theGrid.SaveID;
		}

		private int AssignNextAltSaveID(LOR4Timings theGrid)
		{
			if (theGrid.AltSaveID < 0)
			{
				int newASI = HighestAltSaveID + 1;
				theGrid.AltSaveID = newASI;
				//AllMembers.ByAltSaveID.Add(theGrid);
				HighestAltSaveID = newASI;
			}
			return theGrid.AltSaveID;
		}

		private int AssignNextTrackNumber(LOR4Track theTrack)
		{
			if (theTrack.TrackNumber < 1)
			{
				int newTN = Tracks.Count - 1;
				theTrack.SetID(newTN);
				theTrack.SetIndex(newTN);

			}
			return Tracks.Count;
		}

		private int AssignNextAltTrackIndex(LOR4Track theTrack)
		{
			if (theTrack.AltID < 0)
			{
				int newATN = HighestAltTrackNumber + 1;
				theTrack.AltTrackID = newATN;
			}
			return theTrack.AltTrackID;
		}

		public int HighestSaveID
		{
			get
			{
				// Remember, Timing Grids start at -> 0 <-
				return TimingGrids.Count - 1;
			}
		}

		public int HighestTrackNumber
		{
			get
			{
				// Remember, Tracks start counting at -> 1 <- (not zero)
				return Tracks.Count - 1;
			}
		}



		public override iLOR4Member Clone()
		{
			LOR4Sequence seqOut = (LOR4Sequence)Clone();
			seqOut.animation = animation.Clone();
			seqOut.animation.parentSequence = seqOut;
			seqOut.filename = filename;
			seqOut.lineCount = lineCount;
			seqOut.info = info.Clone();
			seqOut.LOR4SequenceType = LOR4SequenceType;
			seqOut.videoUsage = videoUsage;
			for (int idx = 0; idx < AllMembers.BySavedIndex.Count; idx++)
			{
				LOR4MemberType mt = AllMembers.BySavedIndex[idx].MemberType;
				if (mt == LOR4MemberType.Channel)
				{
					LOR4Channel newCh = (LOR4Channel)AllMembers.BySavedIndex[idx].Clone();
					seqOut.AllMembers.Add(newCh);
				}
			}
			return seqOut;
		}

		public override iLOR4Member Clone(string newName)
		{
			LOR4Sequence seqOut = (LOR4Sequence)this.Clone();
			ChangeName(newName);
			return seqOut;
		}

		public void MoveTrackByNumber(int oldTrackNumber, int newTrackNumber)
		{
			MoveTrackByIndex(oldTrackNumber - 1, newTrackNumber - 1);
		}
		public void MoveTrackByIndex(int oldIndex, int newIndex)
		{
			// IMPORTANT NOTE:
			// POSITIONS ARE THE INDEX (ZERO TO COUNT-1)
			// THEY ARE **NOT** THE TRACK NUMBER (ONE TO COUNT) !!
			string info = "";

			// Sanity Checks
			if ((oldIndex >= 0) &&
				(oldIndex < Tracks.Count) &&
				(newIndex >= 0) &&
				(newIndex <= Tracks.Count) &&
				(newIndex != oldIndex))
			{
				List<LOR4Track> tracksNew = new List<LOR4Track>();
				int newSpot = 0;
				for (int i = 0; i < Tracks.Count; i++)
				{
					if (i == newIndex)
					{
						info = Tracks[oldIndex].Name + " shifted from " + oldIndex.ToString() + " to " + newSpot.ToString();
						Debug.WriteLine(info);
						tracksNew.Add(Tracks[oldIndex]);
						Tracks[oldIndex].SetIndex(newSpot);
					}
					else
					{
						if (i != oldIndex)
						{
							info = Tracks[i].Name + " shifted from " + i.ToString() + " to " + newSpot.ToString();
							Debug.WriteLine(info);
							tracksNew.Add(Tracks[i]);
							Tracks[i].SetIndex(newSpot);
						}
					}
					newSpot++;
				}
				Tracks = tracksNew;
			}
		} // End MoveTrack

		public int LongestMember
		{
			get
			{
				int longest = 0;
				for (int i = 0; i < AllMembers.Count; i++)
				{
					if (AllMembers[i].Centiseconds > longest)
					{
						longest = AllMembers[i].Centiseconds;
					}
					if (longest > LOR4Admin.MAXCentiseconds)
					{
						string m = "WARNING!  Member " + AllMembers[i].Name + " is over 60 minutes!";
						Fyle.WriteLogEntry(m, "Warning");
						if (Debugger.IsAttached)
						{
							System.Diagnostics.Debugger.Break();
						}
					}
					if (longest > myCentiseconds)
					{
						string m = "ERROR!  Member " + AllMembers[i].Name + " is longer than the sequence!";
						Fyle.WriteLogEntry(m, "Error");
						if (Debugger.IsAttached)
						{
							System.Diagnostics.Debugger.Break();
						}
					}
				}
				return longest;
			}
		}

		public int LastEffect
		{
			get
			{
				int last = 0;
				for (int i = 0; i < Channels.Count; i++)
				{
					if (Channels[i].effects.Count > 0)
					{
						for (int e = 0; e < Channels[i].effects.Count; e++)
						{
							if (Channels[i].effects[e].endCentisecond > last)
							{
								last = Channels[i].effects[e].endCentisecond;
								if (last > LOR4Admin.MAXCentiseconds)
								{
									string m = "WARNING!  Last Effect on Channel " + Channels[i].Name + " is past 60 minutes!";
									Fyle.WriteLogEntry(m, "Warning");
									if (Debugger.IsAttached)
									{
										System.Diagnostics.Debugger.Break();
									}
								}
								if (last > myCentiseconds)
								{
									string m = "ERROR! Last effect on Channel " + Channels[i].Name + " is past the end of the sequence!";
									Fyle.WriteLogEntry(m, "Error");
									if (Debugger.IsAttached)
									{
										System.Diagnostics.Debugger.Break();
									}
								}
							}
						}
					}
				}
				return last;
			}
		} // End Last Effect

		public int LastFreeformTiming
		{
			//! Important Note: Returns the last FREEFORM timing
			get
			{
				int last = 0;
				for (int i = 0; i < TimingGrids.Count; i++)
				{
					if (TimingGrids[i].TimingGridType == LOR4TimingGridType.Freeform)
					{
						if (TimingGrids[i].timings.Count > 0)
						{
							for (int e = 0; e < TimingGrids[i].timings.Count; e++)
							{
								if (TimingGrids[i].timings[e] > last)
								{
									last = TimingGrids[i].timings[e];
									if (last > LOR4Admin.MAXCentiseconds)
									{
										string m = "WARNING!  Last Timing in Grid " + TimingGrids[i].Name + " is past 60 minutes!";
										Fyle.WriteLogEntry(m, "Warning");
										if (Debugger.IsAttached)
										{
											System.Diagnostics.Debugger.Break();
										}
									}
									if (last > myCentiseconds)
									{
										string m = "ERROR! Last Timing in Grid " + TimingGrids[i].Name + " is past the end of the sequence!";
										Fyle.WriteLogEntry(m, "Error");
										if (Debugger.IsAttached)
										{
											System.Diagnostics.Debugger.Break();
										}
									}
								}
							}
						}
					}
				}
				return last;
			}
		} // End Last Timing

		public override int color
		{
			get { return LOR4Admin.LORCOLOR_MULTI; }
			set { int ignore = value; }
		}

		public override System.Drawing.Color Color
		{
			get { return LOR4Admin.Color_LORtoNet(this.color); }
			set { System.Drawing.Color ignore = value; }
		}

		public void TimingsResetWritten()
		{
			foreach (LOR4Timings grid in TimingGrids)
			{
				grid.AltID = LOR4Admin.UNDEFINED;
				//member.Written = false;
			}
			HighestAltSaveID = LOR4Admin.UNDEFINED;
		}

		//! END SEQUENCE CLASS
	} // end sequence class
} // end namespace LOR4