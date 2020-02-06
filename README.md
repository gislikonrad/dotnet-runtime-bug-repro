# Reproduction of dotnet runtime issue

This is a repo to reproduce issue [31862](https://github.com/dotnet/runtime/issues/31862) in the dotnet/runtime repo. 

## Reproduction environment

If this matters at all

* Microsoft Windows 10 Enterprise
* Visual Studio 2019 16.4.4 debugger
* Both projects run simultaniously
* Both projects run directly (not iisexpress)
* Request GET - http://localhost:5000
* View both consoles and see the descrepency

## Hack

Lines 27-30 in the Startup class of the Tracing project include commented out code that mitigates this issue using reflection.