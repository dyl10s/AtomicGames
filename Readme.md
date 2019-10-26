# AORTS C# Starter Kit

This is a starter kit for the 2019 AO RTS, built with C# and .NET Core. It includes:

 - simple AI that randomly walks units around
 - unit test project with a couple examples
 - vscode configuration for build/test/debug

## Get Started

- Install [.net core](https://dotnet.microsoft.com/download/dotnet-core/3.0) _(global.json specifies dotnet v3.0.100, which was the latest stable at the time of this writing.)_
- Check out this repo, then `dotnet restore` to install dependencies.
- To run the app, `dotnet run` from the _ai_ directory. (it defaults to port 9090; specify another with e.g. `dotnet run 9091`)
- To run unit tests, `dotnet test` from the _ai.test_ directory.

## VS Code

Configuration is included for [Visual Studio Code](https://code.visualstudio.com/), including _build_ and _test_ tasks, and a launch configuration (e.g. F5 to debug). The _run test_ and _debug test_ buttons within the editor work, too.

## What's next?

- Take a look at the UnitStrategyFactory.
- See if you can improve the exploration strategy! The random moves it's currently making are not a great way to explore the map.
- Try to collect resources to increase your score and ability to build units.
- Maybe create a strategy to build some units?
- Consider an offensive strategy! You can win by blowing up your opponent's base, or you can weaken your opponent by destroying their units.

