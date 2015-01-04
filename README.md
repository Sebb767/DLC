DLC
===

Dynamic Library(function) Caller - A small C# Library to call functions in unmanaged libraries (.dll, .so). 

Features
--------

- **System independent** - works on Windows + Linux. OSX isn't tested, but should work, too.
- **Unload libraries** - if you don't need it any more, unload it. 
- **No caching** - if you have a new version, don't worry about an old, loaded version.
- **Dynamic function search** - you can call any function by name, no need to define them at compile time.

How to use it
-------------

#### Loading a library

Simply include the dlc.dll in your solution and create a new `DLC` class. The constructor hast 3 overloads:

1. Single argument (string library) - the library to load
2. 2 arguments (string x2) - the library to load on windows or linux, respectively.
3. 4 arguments (string x4) - the library to load on windows or linux on x86 or x64, respectively.

The constructor throws a `ClassLoadingException` when something goes wrong. The library is loaded every time you create an instance, so reuse it if you need the same version again. 

#### Retrieving a function handle
You have two options (assuming `DLC dlc = new DLC("native.so");`):

1. Simply call `IntPtr myProc = dlc.getProc("yourProcedure");` to get a pointer to your function. If you have no idea what to do with your pointer, option 2 is for you.
2. The delegate-ty way:
  1. Create a delegate for your native function: `delegate int Add(int a, int b);`
  2. Call `getProcDelegate` like so: `Add add = (Add)dlc.getProcDelegate("nativeAdd", typeof(Add));`. Just use your instance of `Add` as function now: `int myInt = add(3, 5); // 8 ... at least I hope so`.

Both functions throw a `ClassLoadingException` when something goes wrong and an `ObjectDisposedException` if the class was disposed beforehand.

#### Unloading a library
Dispose your class instance via `dlc.Dispose();` or delete the reference and wait for the GC to do it. This class is save to be disposed by the GC and will close the native handle when deleted. If something goes wrong while disposing through, a `ClassLoadingException` will be thrown.

Notes
-----

- There are two public static helper functions:
  - `DLC.isLinux()` returns wether you are running on Mono/Linux or OSX
  - `DLC.x64()` returns wether you are running on an 64bit OS with 64bit Mono.
- The [builds](../../tree/master/DLC/bin) are up-to-date and free to use.

To do
-----

- Implement a bool to suppress an exception when disposing.
- Testing on OSX (I do not own a Mac, so feedback would be nice).


