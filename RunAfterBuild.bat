:: Use your build folder name here
@echo off
SET folder=Build
SET folder=%folder%\AndreTheBoss_Data\Resources
SET source=Assets\Resources\*.xml
xcopy %source% %folder%
