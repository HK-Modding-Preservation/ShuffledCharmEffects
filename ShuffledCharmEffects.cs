using Modding;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using Satchel;

namespace ShuffledCharmEffects {
    public class ShuffledCharmEffects: Mod, ILocalSettings<LocalSettings> {
        new public string GetName() => "ShuffledCharmEffects";
        public override string GetVersion() => "1.1.0.1";
        public override int LoadPriority() => -3;

        public static LocalSettings localData { get; set; } = new();
        public void OnLoadLocal(LocalSettings s) => localData = s;
        public LocalSettings OnSaveLocal() => localData;

        private bool isCalculatingNotches = false;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            On.PlayerData.GetBool += getBoolRecharm;
            On.PlayMakerFSM.OnEnable += editFsm;
            On.GameManager.StartNewGame += onStartGame;
            On.PlayerData.CalculateNotchesUsed += FlagCalculateNotches;
            if(ModHooks.GetMod("Randomizer 4") is Mod) {
                initializeForRando();
            }
        }

        private void FlagCalculateNotches(On.PlayerData.orig_CalculateNotchesUsed orig, PlayerData self) {
            isCalculatingNotches = true;
            orig(self);
            isCalculatingNotches = false;
        }

        private void initializeForRando() {
            RandomizerMod.RC.RandoController.OnExportCompleted += rc => initialSetup();
        }

        private bool getBoolRecharm(On.PlayerData.orig_GetBool orig, PlayerData self, string boolName) {
            if(!isCalculatingNotches && boolName.StartsWith("equippedCharm_")) {
                int origID = int.Parse(boolName.Split('_')[1]);
                try {
                    return orig(self, "equippedCharm_" + localData.shuffledIDs[origID - 1]);
                }
                catch(ArgumentOutOfRangeException) {
                    return orig(self, boolName);
                }
            }
            return orig(self, boolName);
        }

        private void editFsm(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            if(self.gameObject.name == "Charms" && self.FsmName == "UI Charms") {
                FsmState equippedState = self.GetValidState("Equipped?");
                equippedState.RemoveAction(1);
                equippedState.InsertAction(new ReplaceBuildString(self), 1);
            }
        }

        private void onStartGame(On.GameManager.orig_StartNewGame orig, GameManager self, bool permadeathMode, bool bossRushMode) {
            orig(self, permadeathMode, bossRushMode);
            initialSetup();
        }

        private void initialSetup() {
            localData.shuffledIDs = createIDs();
        }

        private List<int> createIDs() {
            System.Random rand = new();
            List<int> numbers = new();
            List<int> output = new();
            int max = ModHooks.GetMod("SFCore") is Mod ? GetMaxCharms() : 40;
            for(int i = 1; i <= max; i++) {
                numbers.Add(i);
            }
            while(numbers.Count > 0) {
                int index = rand.Next(numbers.Count);
                output.Add(numbers[index]);
                numbers.RemoveAt(index);
            }
            return output;
        }

        private int GetMaxCharms() {
            List<Sprite> CustomSprites = typeof(SFCore.CharmHelper).GetField("CustomSprites", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as List<Sprite>;
            return CustomSprites.Count + 40;
        }
    }

    public class ReplaceBuildString: FsmStateAction {
        PlayMakerFSM fsm;

        public ReplaceBuildString(PlayMakerFSM self) {
            fsm = self;
        }

        public override void OnEnter() {
            string fsmString = fsm.FsmVariables.GetFsmString("Item Num String").Value;
            int currentID = int.Parse(fsmString);
            int revertedID = currentID;
            revertedID = ShuffledCharmEffects.localData.shuffledIDs.IndexOf(currentID) + 1;
            fsm.FsmVariables.GetFsmString("PlayerData Var Name").Value = "equippedCharm_" + revertedID;
        }
    }

    public class LocalSettings {
        public List<int> shuffledIDs = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                                       11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                                                       21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                                                       31, 32, 33, 34, 35, 36, 37, 38, 39, 40};
    }
}