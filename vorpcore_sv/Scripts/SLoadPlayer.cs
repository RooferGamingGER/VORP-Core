﻿using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using vorpcore_sv.Utils;

namespace vorpcore_sv.Scripts
{
    public class SLoadPlayer : BaseScript
    {
        public SLoadPlayer()
        {
            EventHandlers["vorp:playerSpawn"] += new Action<Player>(PlayerSpawnFunction);
        }

        private void PlayerSpawnFunction([FromSource] Player source)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    source.TriggerEvent("vorpcharacter:createPlayer");
                }
                else
                {
                    bool isdead = Boolean.Parse(result[0].isdead.ToString());
                    string c_json = result[0].coords;
                    try
                    {
                        Dictionary<string, float> pos = JsonConvert.DeserializeObject<Dictionary<string, float>>(c_json);
                        Vector3 pcoords = new Vector3(pos["x"], pos["y"], pos["z"]);
                        source.TriggerEvent("vorp:initPlayer", pcoords, pos["heading"], isdead);
                    }
                    catch
                    {

                    }

                    // Send Nui Update UI all
                    JsonUiCalls JUC = new JsonUiCalls()
                    {
                        type = "ui",
                        action = "update",
                        moneyquanty = result[0].money,
                        goldquanty = result[0].gold,
                        rolquanty = result[0].rol
                    };

                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(JsonUiCalls));
                    MemoryStream msObj = new MemoryStream();
                    js.WriteObject(msObj, JUC);
                    msObj.Position = 0;
                    StreamReader sr = new StreamReader(msObj);

                    string strjson = sr.ReadToEnd();

                    source.TriggerEvent("vorp:updateUi", strjson);
                }

            }));

        }

    }
}
