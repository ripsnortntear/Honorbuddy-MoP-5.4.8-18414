using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Styx;
using Styx.CommonBot.Database;
using Styx.Helpers;
using Styx.Loaders;
using Styx.CommonBot.AreaManagement;
using Styx.CommonBot.AreaManagement.Triangulation;
using Styx.TreeSharp;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.Frames;
using Styx.Pathing;
using Styx.Pathing.OnDemandDownloading;
using Styx.Patchables;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using Styx.Common;

using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Forms;

namespace BarryDurex
{
	static class QuestHelper
	{
		public static void slog(string message, params object[] args)
		{ Styx.Common.Logging.Write(LogLevel.Quiet, System.Windows.Media.Colors.Salmon, "[QuestHelper]: " + message, args); }
		public static void dlog(string message, params object[] args)
		{ Styx.Common.Logging.WriteDiagnostic(System.Windows.Media.Colors.Salmon, "[QuestHelper]: " + message, args); }
		public static void navlog(string message, params object[] args)
		{ Styx.Common.Logging.WriteDiagnostic(System.Windows.Media.Colors.LawnGreen, "[QuestHelper]: " + message, args); }

		//private static int MinDistToPools = 7;
		//private static int MaxDistToMove = 40;
		//private static int TraceStep = 50;

		#region Lightning Pool Behaviors - not yet tested!

		public static void AvoidEnemyAOE(WoWPoint location, List<WoWDynamicObject> Objects, string Aura, int TraceStep)
		{
			if (Objects == null)
			{ dlog("no Lightning Pools found .."); return; }
			dlog("found {0} {1}! start RayCast ..", Objects.Count, Aura);

			int MinDistToPools = (int)(Objects[0].Radius * 1.6f);
			int MaxDistToMove = MinDistToPools * 2;

			// get save location
			WoWPoint newP = getSaveLocation(location, Objects, MinDistToPools, MaxDistToMove, TraceStep);

			if (newP == WoWPoint.Empty)
			{
				// no save location found, move 2sec Strafe Left
				WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(2));
			}
			else
			{
				// move to save location
				using (StyxWoW.Memory.ReleaseFrame(true))
				{
					while (StyxWoW.Me.HasAura(Aura) && StyxWoW.Me.Location.Distance(newP) > 0.2f)
					{
						Navigator.MoveTo(newP);
					}
				}
			}

			WoWMovement.MoveStop();
			if (StyxWoW.Me.CurrentTargetGuid != 0)
				StyxWoW.Me.CurrentTarget.Face();

			//Styx.CommonBot.Blacklist.Add(
			//Styx.CommonBot.Profiles.Blackspot vv = new Styx.CommonBot.Profiles.Blackspot(
		}

		private static bool wlog(WoWDynamicObject obj)
		{ dlog("add pool - dis2D: {0}", obj.Distance2D); return true; }
		public static List<WoWDynamicObject> getLightningPoolList
		{
			get
			{
				return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
						orderby lp.Distance2D ascending
						where lp.Entry == 129657
						where wlog(lp)
						select lp).ToList();
			}
		}

		public static List<WoWDynamicObject> getCausticPitchList
		{
			get
			{
				return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
						orderby lp.Distance2D ascending
						where lp.Entry == 126336
						where wlog(lp)
						select lp).ToList();
			}
		}
		
		public static List<WoWDynamicObject> getIgniteFuelList
		{
			get
			{
				return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
						orderby lp.Distance2D ascending
						where lp.Entry == 135862
						where wlog(lp)
						select lp).ToList();
			}
		}
		
		public static List<WoWDynamicObject> getHotFootList
		{
			get
			{
				return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
						orderby lp.Distance2D ascending
						where lp.Entry == 129556
						where wlog(lp)
						select lp).ToList();
			}
		}

		public static List<WoWDynamicObject> getVenomSplashList
		{
			get
			{
				return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
						orderby lp.Distance2D ascending
						where lp.Entry == 79607
						where wlog(lp)
						select lp).ToList();
			}
		}
		
		public static List<WoWDynamicObject> getSolarBeamList
		{
			get
			{
				return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
						orderby lp.Distance2D ascending
						where lp.Entry == 129888
						where wlog(lp)
						select lp).ToList();
			}
		}

		#region RayCast

		private static WoWPoint getSaveLocation(WoWPoint Location, List<WoWDynamicObject> badObjects, int minDist, int maxDist, int traceStep)
		{
			navlog("Navigation: Looking for save Location around {0}.", Location);

			try
			{
				//float _PIx2 = 3.14159f * 2f;
				float _PIx2 = ((float)new Random().Next(1, 80) * (1.248349f + (float)new Random().NextDouble()));

				for (int i = 0, x = minDist; i < traceStep && x < maxDist; i++)
				{
					WoWPoint p = Location.RayCast((i * _PIx2) / traceStep, x);

					p.Z = getGroundZ(p);

					if (p.Z != float.MinValue && StyxWoW.Me.Location.Distance2D(p) > 1 &&
						(badObjects.FirstOrDefault(_obj => _obj.Location.Distance2D(p) <= minDist) == null) &&
						//(ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Location.Distance2D(p) < 20 && u.IsAlive && !u.Combat) == null) &&
						Navigator.GeneratePath(StyxWoW.Me.Location, p).Length != 0)
					{
						if (getHighestSurroundingSlope(p) < 1.2f)
						{
							navlog("Navigation: Moving to {0}. Distance: {1}", p, Location.Distance(p));
							return p;
						}
					}

					if (i == (traceStep - 1))
					{
						i = 0;
						x++;
					}
				}
			}
			catch (Exception ex)
			{ Logging.WriteException(ex); }


			dlog(" - No valid points returned by RayCast ...");
			return WoWPoint.Empty;

		}

		/// <summary>
		/// Credits to exemplar.
		/// </summary>
		/// <returns>Z-Coordinates for PoolPoints so we don't jump into the water.</returns>
		private static float getGroundZ(WoWPoint p)
		{
			WoWPoint ground = WoWPoint.Empty;

			GameWorld.TraceLine(new WoWPoint(p.X, p.Y, (p.Z + 100)), new WoWPoint(p.X, p.Y, (p.Z - 5)), GameWorld.CGWorldFrameHitFlags.HitTestGroundAndStructures/* | GameWorld.CGWorldFrameHitFlags.HitTestBoundingModels | GameWorld.CGWorldFrameHitFlags.HitTestWMO*/, out ground);

			if (ground != WoWPoint.Empty)
			{
				navlog(" - Ground Z: {0}.", ground.Z);
				return ground.Z;
			}
			dlog(" - Ground Z returned float.MinValue.");
			return float.MinValue;
		}

		/// <summary>
		/// Credits to funkescott.
		/// </summary>
		/// <returns>Highest slope of surrounding terrain, returns 100 if the slope can't be determined</returns>
		private static float getHighestSurroundingSlope(WoWPoint p)
		{
			navlog("Navigation: Sloapcheck on Point: {0}", p);
			float _PIx2 = 3.14159f * 2f;
			float highestSlope = -100;
			float slope = 0;
			int traceStep = 15;
			float range = 0.5f;
			WoWPoint p2;
			for (int i = 0; i < traceStep; i++)
			{
				p2 = p.RayCast((i * _PIx2) / traceStep, range);
				p2.Z = getGroundZ(p2);
				slope = Math.Abs(getSlope(p, p2));
				if (slope > highestSlope)
				{
					highestSlope = (float)slope;
				}
			}
			navlog(" - Highslope {0}", highestSlope);
			return Math.Abs(highestSlope);
		}

		/// <summary>
		/// Credits to funkescott.
		/// </summary>
		/// <param name="p1">from WoWPoint</param>
		/// <param name="p2">to WoWPoint</param>
		/// <returns>Return slope from WoWPoint to WoWPoint.</returns>
		private static float getSlope(WoWPoint p1, WoWPoint p2)
		{
			float rise = p2.Z - p1.Z;
			float run = (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

			return rise / run;
		}
		#endregion

		#endregion

		#region Behemoth / Cracklefang -Behavior
		/// <summary>
		/// this behavior will move the bot StrafeRight/StrafeLeft only if enemy is casting and we needed to move!
		/// Credits to BarryDurex.
		/// </summary>
		/// <param name="EnemyAttackRadius">EnemyAttackRadius or 0 for move Behind</param>
		public static void AvoidEnemyCast(WoWUnit Unit, float EnemyAttackRadius, float SaveDistance)
		{
			if (!StyxWoW.Me.IsFacing(Unit))
			{ Unit.Face(); }

			float BehemothRotation = getPositive(Unit.RotationDegrees);
			float invertEnemyRotation = getInvert(BehemothRotation);

			WoWMovement.MovementDirection move = WoWMovement.MovementDirection.None;

			if (getPositive(StyxWoW.Me.RotationDegrees) > invertEnemyRotation)
			{ move = WoWMovement.MovementDirection.StrafeRight; }
			else
			{ move = WoWMovement.MovementDirection.StrafeLeft; }

			using (StyxWoW.Memory.ReleaseFrame(true))
			{
				while (Unit.Distance2D <= SaveDistance && Unit.IsCasting && ((EnemyAttackRadius == 0 && !StyxWoW.Me.IsSafelyBehind(Unit)) ||
					(EnemyAttackRadius != 0 && Unit.IsSafelyFacing(StyxWoW.Me, EnemyAttackRadius)) || Unit.Distance2D <= 2 ))
				{
					WoWMovement.Move(move);
					Unit.Face();		   
				}
			}
			WoWMovement.MoveStop();
		}

		private static float getInvert(float f)
		{
			if (f < 180)
				return (f + 180);
			//else if (f >= 180)
			return (f - 180);
		}

		private static float getPositive(float f)
		{
			if (f < 0)
				return (f + 360);
			return f;
		}
		#endregion
	}
}

namespace KatzerleAvoid
{
	static class RunAwayCode
	{
		/// Runs away from an Enemy 
		/// </summary>
		/// <param name="Enemy">the Enemy to run away from</param>
		/// <param name="SpellID">the Spell ID which the Enemy is casting</param>
		/// <param name="MinDistToEnemy">Minimum Distance to Run away from the Enemy</param>
		/// <param name="TraceStep">How many Checks in one Round</param>
		/// <param name="ScanDistance">Distance of the Scans to each other</param>
		public static void FleeingFromEnemy(WoWUnit Enemy, int SpellID, int MinDistToEnemy, int TraceStep, int ScanDistance)
		{
			int MaxDistToMove = MinDistToEnemy * 2;
			Logging.Write(Colors.MediumPurple, "[Brodies Plugin] Fleeing from {0}", Enemy.Name);
			
			Lua.DoString("RunMacroText(\"/stopcasting\");");
			
			// get save location - This Function is is also explained - Brodieman
			WoWPoint newP = getSaveFleeingLocationSingular(Enemy, MinDistToEnemy, MaxDistToMove, TraceStep, ScanDistance, SpellID);

			if (newP == WoWPoint.Empty)
			{
				// no save location found, turn around and move 2sec Forward
				if (StyxWoW.Me.IsFacing(Enemy))
					Navigator.MoveTo(getLocationBehindUnit(StyxWoW.Me));
				WoWMovement.Move(WoWMovement.MovementDirection.Forward, TimeSpan.FromSeconds(2));
			}
			else
			{
				// move to save location
				using (StyxWoW.Memory.ReleaseFrame(true))
				{
					while (StyxWoW.Me.Location.Distance(newP) > 4)
					{
						switch (StyxWoW.Me.Class)
						{
							case WoWClass.Mage:
								if (SpellManager.CanCast(1953) && StyxWoW.Me.IsFacing(newP) && newP.Distance(StyxWoW.Me.Location) > 20)
									CastSafe(1953, StyxWoW.Me, false);
								break;
							case WoWClass.Monk:
								if (SpellManager.CanCast(109132) && StyxWoW.Me.IsFacing(newP) && newP.Distance(StyxWoW.Me.Location) > 20)
									CastSafe(109132, StyxWoW.Me, false);
								break;
							default:
								break;
						}

						if (StyxWoW.Me.IsSwimming)
							WoWMovement.ClickToMove(newP);
						else
							Navigator.MoveTo(newP);
						if (ToonInvalid) return;
						if (SpellID != 0 && (!Enemy.IsCasting || Enemy.CastingSpellId != SpellID) && !Enemy.HasAura(SpellID))
						{
							Logging.Write(Colors.MediumPurple, "[Brodies Plugin] {0} has stopped casting", Enemy.Name);
							WoWMovement.MoveStop();
							return;
						}
						if (StyxWoW.Me.Location.Distance(Enemy.Location) > (MinDistToEnemy + 10)) //dont run too far away because then the Enemy will reset
						{
							Logging.Write(Colors.MediumPurple, "[Brodies Plugin] reached save Location");
							WoWMovement.MoveStop();
							return;
						}
					}
				}
			}
			Logging.Write(Colors.MediumPurple, "[Brodies Plugin] reached save Location");
			WoWMovement.MoveStop();
		}
		
		/// <summary>
		/// Get a Save Location away from the Enemy
		/// </summary>
		/// <param name="Enemy">The Enemy</param>
		/// <param name="minDist">Minimum Distance to the Enemy</param>
		/// <param name="maxDist">Maximum Distance to the Enemy</param>
		/// <param name="TraceStep">How many Checks in one Round</param>
		/// <param name="ScanDistance">Distance of the Scans to each other</param>
		/// <returns>Hopefully a save Point or WoWPoint.Empty</returns>
		public static WoWPoint getSaveFleeingLocationSingular(WoWUnit Enemy, int minDist, int maxDist, int TraceStep, int ScanDistance, int SpellID)
		{
			try
			{
				bool checkForEnemysAround = true;
				DateTime startFind = DateTime.Now;
				int countPointsChecked = 0;
				int countFailToPointNav = 0;
				int countFailSafe = 0;
				double furthestNearMobDistSqr = 0f;
				bool ChooseSafestAvailable = true;
				WoWPoint pFurthest = WoWPoint.Empty;
				WoWPoint pEnemy = Enemy.GetTraceLinePos();

				//Line of Sight
				int countFailToPointLoS = 0;
				int countFailToMobLoS = 0;
				bool CheckLineOfSight = true;
				bool CheckLineOfSightEnemy = true;

				WoWPoint pSavePoint = new WoWPoint();
				List<WoWPoint> mobLocations = AllEnemyMobLocationsToCheck(Enemy);
				float arcIncrement = ((float)Math.PI * 2) / TraceStep;
				float _PIx2 = ((float)new Random().Next(1, 80) * (1.248349f + (float)new Random().NextDouble()));
				double minSafeDistSqr = 15 * 15;

				float baseDestinationFacing = Enemy == null ?
					StyxWoW.Me.RenderFacing + (float)Math.PI
					: Styx.Helpers.WoWMathHelper.CalculateNeededFacing(Enemy.Location, StyxWoW.Me.Location);

				Logging.Write(Colors.MediumPurple, "BP/RK RayCast: search near {0:F0}d @ {1:F1} yds for Unit free area", WoWMathHelper.RadiansToDegrees(baseDestinationFacing), minDist);
				for (int arcIndex = 0; arcIndex < TraceStep; arcIndex++)
				{
					float checkFacing = WoWMathHelper.DegreesToRadians((float)new Random().Next(1, 360));
					if (Enemy.Entry == 50821 || Enemy.Entry == 50817 || Enemy.Entry == 50822 || Enemy.Entry == 50816 ||
						Enemy.Entry == 50811 || Enemy.Entry == 50808 || Enemy.Entry == 50820)
					{
						checkFacing = baseDestinationFacing;
						if ((arcIndex & 1) == 0)
							checkFacing += arcIncrement * (arcIndex >> 1);
						else
							checkFacing -= arcIncrement * ((arcIndex >> 1) + 1);
					}

					for (float distFromOrigin = minDist; distFromOrigin <= maxDist; distFromOrigin += ScanDistance)
					{
						countPointsChecked++;
						if (ToonInvalid) return StyxWoW.Me.Location;
						if ((SpellID != 0 && (!Enemy.IsCasting || Enemy.CastingSpellId != SpellID)))
						{
							Logging.Write(Colors.MediumPurple, "[Brodies Plugin] {0} don't cast any more", Enemy.Name);
							WoWMovement.MoveStop();
							return StyxWoW.Me.Location;
						}

						pSavePoint = pEnemy.RayCast(checkFacing, distFromOrigin);
						pSavePoint.Z = getGroundZ(pSavePoint);

						if (pSavePoint.Z == float.MinValue)
						{
							Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: Location failed getGroundZ for degrees={0:F1} dist={1:F1}", WoWMathHelper.RadiansToDegrees(checkFacing), distFromOrigin);
							countFailToPointNav++;
							continue;
						}

						if (Navigator.GeneratePath(StyxWoW.Me.Location, pSavePoint).Length == 0 || StyxWoW.Me.Location.Distance2D(pSavePoint) < 1 || !Navigator.CanNavigateFully(StyxWoW.Me.Location, pSavePoint))
						{
							Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: Location failed navigation check for degrees={0:F1} dist={1:F1}", WoWMathHelper.RadiansToDegrees(checkFacing), distFromOrigin);
							countFailToPointNav++;
							continue;
						}

						// Check Line of Sight
						if (CheckLineOfSight && !Styx.WoWInternals.World.GameWorld.IsInLineOfSight(StyxWoW.Me.GetTraceLinePos(), pSavePoint))
						{
							Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: Failed line of sight check for degrees={0:F1} dist={1:F1}", WoWMathHelper.RadiansToDegrees(checkFacing), distFromOrigin);
							countFailToPointLoS++;
							continue;
						}

						if (CheckLineOfSightEnemy && !Styx.WoWInternals.World.GameWorld.IsInLineOfSpellSight(pSavePoint, Enemy.GetTraceLinePos()))
						{
							if (!Styx.WoWInternals.World.GameWorld.IsInLineOfSight(pSavePoint, Enemy.GetTraceLinePos()))
							{
								Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: Failed Unit line of sight check for degrees={0:F1} dist={1:F1}", WoWMathHelper.RadiansToDegrees(checkFacing), distFromOrigin);
								countFailToMobLoS++;
								continue;
							}
						}

						if (checkForEnemysAround)
						{
							WoWPoint ptNearest = NearestMobLoc(pSavePoint, mobLocations);
							if (ptNearest == WoWPoint.Empty)
							{
								if (furthestNearMobDistSqr < minSafeDistSqr)
								{
									furthestNearMobDistSqr = minSafeDistSqr;
									pFurthest = pSavePoint;	 // set best available if others fail
								}
							}
							else
							{
								double mobDistSqr = pSavePoint.Distance2DSqr(ptNearest);
								if (furthestNearMobDistSqr < mobDistSqr)
								{
									furthestNearMobDistSqr = mobDistSqr;
									pFurthest = pSavePoint;	 // set best available if others fail
								}
								if (mobDistSqr <= minSafeDistSqr)
								{
									Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: Safe Location failed, Hostile Mobs around degrees={0:F1} dist={1:F1}", WoWMathHelper.RadiansToDegrees(checkFacing), distFromOrigin);
									countFailSafe++;
									continue;
								}
							}
						}
						Logging.Write(Colors.MediumPurple, "BP/RK RayCast: Found Unit-free location ({0:F1} yd radius) at degrees={1:F1} dist={2:F1} on point check# {3} at {4}, {5}, {6}", 15, WoWMathHelper.RadiansToDegrees(checkFacing), distFromOrigin, countPointsChecked, pSavePoint.X, pSavePoint.Y, pSavePoint.Z);
						Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: processing took {0:F0} ms", (DateTime.Now - startFind).TotalMilliseconds);
						return pSavePoint;
					}
				}

				Logging.Write(Colors.MediumPurple, "BP/RK RayCast: No Unit-free location ({0:F1} yd radius) found within {1:F1} yds ({2} checked, {3} nav, {4} not safe, LoS {5}, Enemy LoS {6})", 15, maxDist, countPointsChecked, countFailToPointNav, countFailSafe, countFailToPointLoS, countFailToMobLoS);
				if (ChooseSafestAvailable && pFurthest != WoWPoint.Empty)
				{
					Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: choosing best available spot in {0:F1} yd radius where closest Unit is {1:F1} yds", 15, Math.Sqrt(furthestNearMobDistSqr));
					Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: processing took {0:F0} ms", (DateTime.Now - startFind).TotalMilliseconds);
					return ChooseSafestAvailable ? pFurthest : WoWPoint.Empty;
				}
				Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: processing took {0:F0} ms", (DateTime.Now - startFind).TotalMilliseconds);
				return WoWPoint.Empty;
			}
			catch (Exception ex)
			{ Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK RayCast: {0}", ex.Message); }
			Logging.Write(Colors.MediumPurple, "BP/RK RayCast: No valid points returned by RayCast ...");
			return WoWPoint.Empty;
		}
		
		/// <summary>
		/// Casts a Spell on a Unit
		/// </summary>
		/// <param name="spellId">The SpellID as int</param>
		/// <param name="Unit">The Enemy</param>
		/// <param name="wait">true and he will wait till the Cast is finished</param>
		/// <returns>true if the cast was sucessfull</returns>
		public static bool CastSafe(int spellID, WoWUnit Unit, bool wait)
		{
			bool SpellSuccess = false;

			if (!SpellManager.HasSpell(spellID))
			{
				Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK: I don't have Spell {0}", spellID);
				return false;
			}

			WoWMovement.MoveStop();
			if (Unit != StyxWoW.Me)
			{
				Unit.Target();
				if (!StyxWoW.Me.IsFacing(Unit))
				{
					Unit.Face();
				}
			}

			if (!SpellManager.CanCast(spellID))
			{
				/*Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK: cannot cast spell '{0}' yet - cd={1}, gcd={2}, casting={3} ",
					SpellManager.Spells[spellID].Name,
					SpellManager.Spells[spellID].Cooldown,
					SpellManager.GlobalCooldown,
					Me.IsCasting
					);*/
				return false;
			}
			Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK: * Cast Distance: {0}", Unit.Location.Distance(StyxWoW.Me.Location).ToString());
			SpellSuccess = SpellManager.Cast(spellID, Unit);
			Logging.Write(Colors.MediumPurple, "BP/RK: * {0} - {1}", spellID, SpellSuccess);
			Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK Developer Log: Global Cooldown running - Variable in HB {0}", SpellManager.GlobalCooldown);
			return SpellSuccess;
		}
		
		public static bool ToonInvalid
		{
			get
			{
				if (StyxWoW.Me == null || !StyxWoW.IsInGame || StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost || InPetCombat()) return true;
				return false;
			}
		}
		
		public static WoWPoint getLocationBehindUnit(WoWUnit Unit)
		{
			return Unit.Location.RayCast(Unit.Rotation + WoWMathHelper.DegreesToRadians(180), 8f);
		}

		/// <summary>
		/// Returns a List of all Enemys around me exept the Enemy
		/// </summary>
		public static List<WoWPoint> AllEnemyMobLocationsToCheck(WoWUnit Enemy)
		{
			return (from u in AllEnemyMobs
						where u != Enemy && u.Distance2DSqr < (65 * 65) && !u.Combat
						select u.Location).ToList();
		}

		/// <summary>
		/// Returns a List of all alive and hostile Enemys around me
		/// </summary>
		public static IEnumerable<WoWUnit> AllEnemyMobs
		{
			get
			{
				return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => !o.IsDead && o.IsHostile && !o.Combat).OrderBy(u => u.Distance).ToList();

			}
		}

		public static float getGroundZ(WoWPoint p)
		{
			WoWPoint ground = WoWPoint.Empty;

			GameWorld.TraceLine(new WoWPoint(p.X, p.Y, (p.Z + 100)), new WoWPoint(p.X, p.Y, (p.Z - 5)), GameWorld.CGWorldFrameHitFlags.HitTestGroundAndStructures/* | GameWorld.CGWorldFrameHitFlags.HitTestBoundingModels | GameWorld.CGWorldFrameHitFlags.HitTestWMO*/, out ground);

			if (ground != WoWPoint.Empty)
			{
				Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK: Ground Z: {0}.", ground.Z);
				return ground.Z;
			}
			Logging.WriteDiagnostic(Colors.MediumPurple, "BP/RK: Ground Z returned float.MinValue.");
			return float.MinValue;
		}

		public static WoWPoint NearestMobLoc(WoWPoint p, IEnumerable<WoWPoint> mobLocs)
		{
			if (!mobLocs.Any())
				return WoWPoint.Empty;

			return mobLocs.OrderBy(u => u.Distance2DSqr(p)).First();
		}
		
		public static bool InPetCombat()
		{
			List<string> cnt = Lua.GetReturnValues("dummy,reason=C_PetBattles.IsTrapAvailable() return dummy,reason");

			if (cnt != null) { if (cnt[1] != "0") return true; }
			return false;
		}
	}
}