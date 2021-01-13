See [Document](https://macacagames.github.io/MacacaUtility/) for more detail.

# Welcome to Macaca Utility

## Installation

### Option 1: Unity Package manager
Add it to your editor's `manifest.json` file like this:
```json
    {
    "dependencies": {
        "com.macacagames.utility": "https://github.com/MacacaGames/MacacaUtility.git#1.0.0",
    }
}
```

You can remove the #1.0.0 to use the latest version (unstable)

### Option 2: Git SubModule
Note: GameSystem is dependency with Rayark.Mast, so also add it in git submodule.

```bash
git submodule add https://github.com/MacacaGames/MacacaUtility.git Assets/MacacaUtility
```
