# unity-cntk
Unity 2017 (.net 4.6 support!) with CNTK 2.0 demo (only evaluation API's)

See thread https://github.com/Microsoft/CNTK/issues/960 for some history 

Tested With/Requires
- Unity 2017 beta 5 (with .net 4.6 enabled), see https://unity3d.com/unity/beta
- CNTK GPU 2.0rc 2 https://cntk.ai/dlwg-2.0.rc2.html

Use
1) download unity and cntk libs from links above
2) copy the following DLL's to C:\Program Files\Unity 2017.1.0b5\Editor 

(see https://github.com/Microsoft/CNTK/wiki/CNTK-Library-Evaluation-on-Windows#using-the-cntk-library-managed-api 
and http://stackoverflow.com/questions/36527985/dllnotfoundexception-in-while-building-desktop-unity-application-using-artoolkit)

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
  
3) clone project
4) run main.unity executable to boot up Unity
5) hit play to see results and checkout the .cs script to see how the API's work

![test](https://cloud.githubusercontent.com/assets/6376127/26030649/fb312fd4-3858-11e7-8e1d-947ac4d7e965.png)
