# CombatBot for Honorbuddy 2.5.11489.748

A custom combat bot for the Honorbuddy platform, focused on clarity, maintainability, and efficient combat automation in World of Warcraft.

---

## Features

- Automated combat, pulling, healing, and following behaviors
- Streamlined party role logic (tank/leader detection)
- Custom targeting filters and combat routines
- Optimized for reliability and clarity
- Easily configurable and extendable

---

## Old vs New

### Previous (Old) Version

- Contained inconsistent formatting and spacing throughout the codebase.
- Included legacy, commented-out code cluttering the logic.
- Complex, deeply nested behaviors with redundant checks.
- Limited comments, making intent and structure harder to follow.
- Event subscriptions could be triggered multiple times.
- Less effective null checking and variable caching.
- Some public interop declarations (`DllImport`) exposed unnecessarily.

### Current (New) Version (as of [32fddf49](https://github.com/ripsnortntear/CombatBot_HB_2.5.11489.748/commit/32fddf49e99458bef1b3fcf94c879c2d78dc4a78))

- **Refactored for clarity:** Spacing, braces, and access modifiers standardized.
- **Legacy code removed:** All commented-out old logic has been deleted.
- **Better encapsulation:** `DllImport` for `GetAsyncKeyState` is now private and static.
- **Improved maintainability:** Extensive comments clarify code sections and logic.
- **Streamlined behaviors:** Redundant checks and deeply nested decorators removed; logic for pull, rest, buff, and combat is now consolidated and readable.
- **Performance optimizations:** Local variable caching for repeated property accesses, improved null checks, and more robust state restoration.
- **Event safety:** Added an `_isInitialized` flag to ensure event subscriptions only occur once.
- **Clear sectioning:** Comments and formatting now clearly separate major behavior sections (combat, rest, following, etc.).

These improvements enhance performance, safety, and readability—making it easier to extend or debug the bot.

---

## Getting Started

1. Place `CombatBot.cs` in your Honorbuddy Bots/Combat/ directory.
2. Close and reload Honorbuddy; select "CombatBot" from the bot dropdown.
3. Configure routines and settings as desired.

---

## Contributing

Pull requests are welcome! Please format code to match the current style and add comments for major logic changes.

---

## License

MIT License—see LICENSE file for details.

---

**Maintained by [ripsnortntear](https://github.com/ripsnortntear)**
