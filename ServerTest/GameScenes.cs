namespace ServerTest
{
    using System;
    using System.Collections.Generic;

    public static class GameScenes
    {
        public static Dictionary<string, SceneID> Scenes;

        static GameScenes()
        {
            Dictionary<string, SceneID> dictionary = new Dictionary<string, SceneID> {
                { 
                    "Assets/Scene/ItemScene.unity",
                    SceneID.ItemScene
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_CorridorModule/AltCorp_CorridorModule.unity",
                    SceneID.AltCorp_CorridorModule
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_CorridorIntersectionModule/AltCorp_CorridorIntersectionModule.unity",
                    SceneID.AltCorp_CorridorIntersectionModule
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/Altcorp_Corridor45TurnModule/AltCorp_Corridor45TurnModule.unity",
                    SceneID.AltCorp_Corridor45TurnModule
                },
                { 
                    "Assets/Scene/Environment/Ship/AltCorp_Shuttle_SARA/AltCorp_Shuttle_SARA.unity",
                    SceneID.AltCorp_Shuttle_SARA
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/ALtCorp_PowerSupply_Module/ALtCorp_PowerSupply_Module.unity",
                    SceneID.ALtCorp_PowerSupply_Module
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_LifeSupportModule/AltCorp_LifeSupportModule.unity",
                    SceneID.AltCorp_LifeSupportModule
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_Cargo_Module/AltCorp_Cargo_Module.unity",
                    SceneID.AltCorp_Cargo_Module
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_CorridorVertical/AltCorp_CorridorVertical.unity",
                    SceneID.AltCorp_CorridorVertical
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_Command_Module/AltCorp_Command_Module.unity",
                    SceneID.AltCorp_Command_Module
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_Corridor45TurnRightModule/AltCorp_Corridor45TurnRightModule.unity",
                    SceneID.AltCorp_Corridor45TurnRightModule
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_StartingModule/AltCorp_StartingModule.unity",
                    SceneID.AltCorp_StartingModule
                },
                { 
                    "Assets/Scene/Environment/Station/Module/Generic/Generic_Debris_Module_JuncRoom001/Generic_Debris_Module_JuncRoom001.unity",
                    SceneID.Generic_Debris_JuncRoom001
                },
                { 
                    "Assets/Scene/Environment/Station/Module/Generic/Generic_Debris_Module_JuncRoom002/Generic_Debris_Module_JuncRoom002.unity",
                    SceneID.Generic_Debris_JuncRoom002
                },
                { 
                    "Assets/Scene/Environment/Station/Module/Generic/Generic_Debris_Module_Corridor001/Generic_Debris_Module_Corridor001.unity",
                    SceneID.Generic_Debris_Corridor001
                },
                { 
                    "Assets/Scene/Environment/Station/Module/Generic/Generic_Debris_Module_Corridor002/Generic_Debris_Module_Corridor002.unity",
                    SceneID.Generic_Debris_Corridor002
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_Airlock_Module/AltCorp_Airlock_Module.unity",
                    SceneID.AltCorp_AirLock
                },
                { 
                    "Assets/Scene/Environment/Station/Module/AltCorp/AltCorp_DockableContainer/AltCorp_DockableContainer.unity",
                    SceneID.AltCorp_DockableContainer
                },
                { 
                    "Assets/Test/Mata_Local/Prefabs01.unity",
                    SceneID.MataPrefabs
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A002/Asteroid_A002.unity",
                    SceneID.Asteroid01
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A003/Asteroid_A003.unity",
                    SceneID.Asteroid02
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A005/Asteroid_A005.unity",
                    SceneID.Asteroid03
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A006/Asteroid_A006.unity",
                    SceneID.Asteroid04
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A008/Asteroid_A008.unity",
                    SceneID.Asteroid05
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A009/Asteroid_A009.unity",
                    SceneID.Asteroid06
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A011/Asteroid_A011.unity",
                    SceneID.Asteroid07
                },
                { 
                    "Assets/Scene/Environment/SolarSystem/Asteroids/Asteroid_A017/Asteroid_A017.unity",
                    SceneID.Asteroid08
                }
            };
            Scenes = dictionary;
        }

        public static class Ranges
        {
            public const int AsteroidsFrom = 0x3e8;
            public const int AsteroidsTo = 0x3ef;
            public const int CelestialBodiesFrom = 1;
            public const int CelestialBodiesTo = 0x13;
            public const int DebrisFrom = 15;
            public const int DebrisTo = 0x12;
        }

        public enum SceneID
        {
            AltCorp_AirLock = 14,
            AltCorp_Cargo_Module = 9,
            AltCorp_Command_Module = 11,
            AltCorp_Corridor45TurnModule = 5,
            AltCorp_Corridor45TurnRightModule = 12,
            AltCorp_CorridorIntersectionModule = 4,
            AltCorp_CorridorModule = 3,
            AltCorp_CorridorVertical = 10,
            AltCorp_DockableContainer = 0x13,
            AltCorp_LifeSupportModule = 8,
            ALtCorp_PowerSupply_Module = 7,
            AltCorp_Ship_Tamara = 2,
            AltCorp_Shuttle_SARA = 6,
            AltCorp_StartingModule = 13,
            Asteroid01 = 0x3e8,
            Asteroid02 = 0x3e9,
            Asteroid03 = 0x3ea,
            Asteroid04 = 0x3eb,
            Asteroid05 = 0x3ec,
            Asteroid06 = 0x3ed,
            Asteroid07 = 0x3ee,
            Asteroid08 = 0x3ef,
            Generic_Debris_Corridor001 = 0x11,
            Generic_Debris_Corridor002 = 0x12,
            Generic_Debris_JuncRoom001 = 15,
            Generic_Debris_JuncRoom002 = 0x10,
            ItemScene = -2,
            MataPrefabs = 20,
            None = -1,
            Slavica = 1
        }
    }
}
