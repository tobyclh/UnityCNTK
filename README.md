# UnityCNTK
#### Deep learning framework for Unity, backed by CNTK
##### Unslash your creativity, built for production

## Update 15/7/2017
Recently I have not been updating this because of personal matter and the fact that Microsoft is considering to give full support 
including training etc. on .NET framework. Will start to work on this again as soon as I can get a clearer vision about where the framework is heading to (rather confusing now if you look at the issue in CNTK .NET. Meanwhile If you need something rn, let me know.


### Planned Features
1. Drag and drop Solution
    - Self-containing, plug and go
    - Core functions require no extra dependency
    - Includes all dependencies for extra
2. In-game AI evaluation
    - Asynchronized Evaluation, Minimize performance hit
    - Parallel Computation
3. Unlimited expensionality
    - Backend with CNTK that unlock the best performance
4. Examples 
    - Various applications to show the potential of CNTK
5. Rich library
    - Support CNTK Model V1, V2
    - Include downloader to common state of the art available deep neural network
6. Built for performance
    - Calculation are all in background thread
    - Paralleled calculation
    - Multi GPU supported
    - Let's Microsoft [explains](https://github.com/Microsoft/CNTK/wiki/Eight-Reasons-to-Switch-from-TensorFlow-to-CNTK) why you should use CNTK instead of tensorflow
7. Crossplatform 
    - written in .Net 4.6
    - Tested in Ubuntu 16.04 and Windows 10, Mac support is in Microsoft's ToDo's
8. Full Source code
    - Implement entirely in C# and Nuget package
    - Welcome to poke around
9. GUI support
10. Test suite
    - Branchmark your neural network in Unity

### Requirement
1. Unity 2017 or later
    - Unity 5.5 or before will NOT work for their lack of .NET 4.6 support


### Includes
1. Easy conversion from Unity classes to CNTK supported value
2. Plenty of examples to get your hand dirty
3. Pretrained Models ready to be deployed
4. Import existing CNTK models
5. Well-documented, properly commented code
6. Data exporter
7. Python data importer
8. GUI and test suite

### Example scenes
1. Image recognition
    - Recognize in-game object (WIP)
2. Facial expression recognition (FER)
    - Recognize fa
    - Requires a front facing camera (Notebook computer will do)
    - 
    - Trained with [Microsoft FER+](https://github.com/Microsoft/FERPlus)

## Getting start
1. Download Unity 2017b or latest
    - required for async support
2. Download [NuGet package of CNTK for windows](https://github.com/Microsoft/CNTK/wiki/NuGet-Package) with GPU support
3. Extract the downloaded files
4. Copy the listed files to your Unity editor .exe location (typically  C:\Program Files\Unity 2017.xxxx\Editor)
- Cntk.Core-2.0rc2.dll
- Cntk.Math-2.0rc2.dll
- libiomp5md.dll
- mkl_cntk_p.dll
- Cntk.Core.Managed-2.0rc2.dll
- Cntk.Core.CSBinding-2.0rc2.dll
- cublas64_80.dll
- cudart64_80.dll
- cudnn64_5.dll
- curand64_80.dll
- cusparse64_80.dll
- nvml.dll 
5. Copy Cntk.Core.Managed-2.0rc3.dll to this project file, under Assets/UnityCNTK/Plugins
6. Run the webcam scene (and see how it doesn't work because we did not implement the correct preprocessing, but the evaluation does work


### Technical Background
This section is dedicated for those who want to understand the models that come with
this problem.

### FAQ
0. What is CNTK  
    Cognitive Toolkit from Microsoft, wonderful open source Deep learning framework.
    Awesome if you care about accuracy, speed and crossplatform support. Here is a [writeup](https:// somewriteup.org) that compares CNTK with some other platforms. 

1. Will I be able to train my XXXX network with this addon?  
    No.
    As of 6/2017, Microsoft only provides Evaluation library in C#, which is why we do NOT have training in C#, this won't change unless Microsoft exposes more API in C#.
    However, we do provide data exporting scripts that can help you export data gathered from Unity to train in C++ / Python.

2. Does this interface support XXX network trained in other framework like tensorflow etc?  
    Not directly, all models have to be in CNTK format.
    However, Microsoft does provide some very well known pretrained network in such format, like VGG-19 etc, and we have a downloader just for that :) 
    There are also easy way that you can convert some models from other script to CNTK format.
    
3. Is legacy models (CNTK V1 Model from brainscript) supported?  
    Yes! We do support both CNTK models V1 and V2.

4. Can I use CNTK without this plugin?  
    Absolutely. CNTK is open-source on [github](https://github.com/Microsoft/CNTK).
    We are merely providing a abstract layer for easy deploying :)
