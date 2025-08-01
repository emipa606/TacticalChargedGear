# GitHub Copilot Instructions for Tactical Charged Gear (Continued)

## Mod Overview and Purpose

**Tactical Charged Gear (Continued)** is an advanced RimWorld mod aimed at enhancing late-game experiences with high-tech equipment. The mod provides a suite of computerised charged weaponry, armor, and turrets, conceptualized as Spacer to Ultra tech level gear. It is designed to enrich gameplay by introducing Tactical Charged Gear (TC Gear) that players can craft, augment, and utilize in strategic scenarios. The mod includes a unique crafting system centered around workbenches and various chips for enhancing weaponry and armor systems.

## Key Features and Systems

- **Advanced Weaponry and Armor**: Includes TC weapons such as pistols, SMGs, shotguns, rifles, and more advanced options like blasters and tactical particle cannons; Melee options include knives, swords, spears, and maces. TC armor offers less protection than standard but optimizes movement and operation under hostile conditions.
  
- **Chips System**: Introduces an enhancement system where chips can be slotted into weapons to improve characteristics such as aiming and damage. It includes general chips (e.g., Control, Aim, Boost, Power) and specialisation chips (e.g., Reflex, Hunter, Heatsink, Dodge).

- **Dedicated Crafting Infrastructure**: A tactical workbench and a chip construction table are integrated for creating and upgrading TC gear.

- **Security Enhancements**: Features improved turrets and mortars, with enhanced operational mechanics and special ammunitions.

- **Toolbox Integration**: Introduces a modifiable toolbox providing tactical enhancements to crafting stations.

## Coding Patterns and Conventions

The mod mostly adheres to typical C# coding conventions, with classes and methods properly encapsulated to manage behavior related to TC Gear, its crafting process, and game compatibility. Key classes like `PelShieldApparel`, `CompTCGRepair`, and `TCBombardment` encapsulate functionality while methods like `DoRepair`, `StartRandomFire`, and `CreateRandomExplosion` define specific behaviors.

## XML Integration

The mod integrates XML for defining in-game elements and configurations. This allows for straightforward mod balancing and includes setting up research techs after Charged Shot. Developers can extend and adjust the XML configurations to introduce new features or tweak existing ones as per design goals.

## Harmony Patching

For mod compatibility and extension of game mechanics, Harmony patches are applied. These patches allow integration without needing to modify the base game code. The `HarmonyPatching` class encapsulates functionality to apply patches efficiently.

## Suggestions for Copilot

- When writing methods for new features or modifying existing ones, consider using best practices like:
  - Proper naming conventions for clarity.
  - Utilizing existing methods as templates for new feature implementations.
  - Writing clean, concise, and well-documented code.

- For XML extensions, look at how existing XML files are structured and emulate those patterns to maintain consistency.

- Utilize the toolbox and Harmony to introduce new gameplay mechanics without directly altering core game files.

- When working on balance, use the XML configurations for chip and weapon stats as a real-time tuning mechanism.

- Suggest integration with existing popular mods by studying how current compatibility is maintained (e.g., with MultiplayerSupport or JecsTools).

- When developing new features, consider open-ended enhancement points to maintain modularity and future extensibility, following patterns seen in `TCGOptions_Initializer` or `Settings`.

## Development Notes

The mod is functional for existing save games and designed with broad compatibility in mind, although workbenches and related items need to be removed if uninstalling the mod to avoid errors from Jecstools elements. Future enhancements are planned, potentially introducing new factions or improving compatibility with other mods such as CE. 

For enhancements or modifications, developers should review compatibility notes and existing integrations to maintain the mod's extensive interoperability with other popular modifications in the RimWorld community.
