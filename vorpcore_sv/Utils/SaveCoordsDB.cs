﻿using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vorpcore_sv.Utils
{
    public class SaveCoordsDB : BaseScript
    {
        public static Dictionary<Player, Tuple<Vector3, float>> LastCoordsInCache = new Dictionary<Player, Tuple<Vector3, float>>();
        public SaveCoordsDB()
        {
            EventHandlers["vorp:saveLastCoords"] += new Action<Player, Vector3, float>(SaveLastCoords);

            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);

            EventHandlers["vorp:ImDead"] += new Action<Player, bool>(OnPlayerDead);

            Tick += saveLastCoordsTick;
        }

        private void OnPlayerDead([FromSource]Player player, bool isDead)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);
            int dead = isDead ? 1 : 0;
            Exports["ghmattimysql"].execute("UPDATE characters SET isdead=? WHERE identifier=?", new[] { dead.ToString(), sid });
        }

        private void OnPlayerDropped([FromSource]Player player, string reason)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);

            try
            {
                Vector3 lastCoords = LastCoordsInCache[player].Item1;
                float lastHeading = LastCoordsInCache[player].Item2;

                UPlayerCoords UPC = new UPlayerCoords()
                {
                    x = lastCoords.X,
                    y = lastCoords.Y,
                    z = lastCoords.Z,
                    heading = lastHeading
                };

                string pos = JsonConvert.SerializeObject(UPC);

                Exports["ghmattimysql"].execute("UPDATE characters SET coords=? WHERE identifier=?", new[] { pos, sid });

                LastCoordsInCache.Remove(player);
            }
            catch
            {

            }
        }

        private void SaveLastCoords([FromSource] Player source, Vector3 lastCoords, float lastHeading)
        {
            LastCoordsInCache[source] = new Tuple<Vector3, float>(lastCoords, lastHeading);
        }

        [Tick]
        private async Task saveLastCoordsTick()
        {
            await Delay(300000);
            foreach (var source in LastCoordsInCache)
            {
                string sid = ("steam:" + source.Key.Identifiers["steam"]);
                try
                {
                    Vector3 lastCoords = source.Value.Item1;
                    float lastHeading = source.Value.Item2;

                    UPlayerCoords UPC = new UPlayerCoords()
                    {
                        x = lastCoords.X,
                        y = lastCoords.Y,
                        z = lastCoords.Z,
                        heading = lastHeading
                    };

                    string pos = JsonConvert.SerializeObject(UPC);

                    Exports["ghmattimysql"].execute("UPDATE characters SET coords=? WHERE identifier=?", new[] { pos, sid });
                }
                catch { continue; }
            }

        }



    }
}
