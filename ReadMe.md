# ShuffledCharmEffects

This mod changes around the effects of every vanilla charm. The shuffling is completely random and is chosen when a savefile is first created.
Opening an existing unrandomized file will retain vanilla charm effects.
All effects and synergies of any charm will be demonstrated by the mimic charm and not by its original source.
Names, locations, and notch costs remain vanilla, only the effects are shuffled.

Some quirks of specific charms include:
- If you die while wearing a fragile charm, it won't break
- If you die while wearing a charm that is imitating a fragile charm:
	- The fragile being imitated will break and become unequippable
	- The equipped charm will stay on and can be equipped freely, but it will have no effect until repaired
- The first three grimmkin flames can be collected regardless of whether the troupe has been summoned
- The charm that imitates Kingsoul can be equipped at any time but will not generate soul until both fragments have been obtained
	- Opening birthplace does still work without fragments

### Mod Interop
- Charm mods such as **Transcendence**, **MidasTouch**, and anything else using SFCore's charm helper will be included if installed
	- These have a known visual glitch where sitting on a bench will appear to equip a mimicked charm or unequip one of the additional charms, but they should mechanically function as expected
	- The additional charms will be shuffled if the mod is even installed, regardless of whether the charms may exist (i.e. disabling Transcendence in Rando)
- It is now possible to shuffle charms in a rando file (which didn't work in this mod's initial release)

*Special thanks to BirdiestBlue for commissioning this mod*