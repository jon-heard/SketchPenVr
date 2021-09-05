cd /d "M:\_projects\SketchPenVr" &msbuild "Assembly-CSharp.csproj" /t:sdvViewer /p:configuration="Debug" /p:platform="Any CPU" /p:SolutionDir="M:\_projects\SketchPenVr" 
exit %errorlevel% 