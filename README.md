# UnityCNTK
#### Deep learning framework for Unity, backed by CNTK
##### Unslash your creativity, built for production

### Planned Features
2. In-game AI evaluation
    - Containable API
3. Unlimited expensionality
    - Backend with CNTK that unlock the best performance
4. Examples 
    - Various applications to show the potential of CNTK
5. Rich library
    - Support CNTK Model V2
    - Include downloader to common state of the art available deep neural network
6. Built for performance
    - Calculation are all in background thread
    - Paralleled calculation
    - GPU Support
7. Crossplatform 
    - written in .Net 4.6
    - in English, that means it should run on most mainstream platforms that Unity supports (tested only on Windows and Ubuntu)
8. Full Source code
    - Entire implementation is C#
    - Poke around and let us know if you find any bug or improvement
9. GUI support
    - 


### Requirement
1. Unity 2017 or later
    - Unity 5.x or before will NOT work for their lack of .NET 4.6 support


### Includes
1. Easy conversion from Unity classes to CNTK supported value
2. Plenty of examples to get your hand dirty
3. Pretrained Models ready to be deployed
4. Import existing CNTK models
5. Well-documented, properly commented code
6. Data exporter
7. Python data importer

### Example scenes
1. Image recognition
    - Recognize in-game object
2. Facial expression recognition (FER)
    - Recognize fa
    - Requires a front facing camera (Notebook computer will do)
    - 
    - Trained with [Microsoft FER+](https://github.com/Microsoft/FERPlus)

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
    No directly, all models have to be in CNTK format.
    However, Microsoft does provide some very well known pretrained network in such format, like VGG-19 etc, and we have a downloader just for that :) 
    There are also easy way that you can convert some models from other script to CNTK format.
    
3. Is legacy models (CNTK V1 Model from brainscript) supported?
    Yes! We do support both CNTK models V1 and V2.

4. Can I achieve 