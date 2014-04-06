# HansKindberg-Build-XmlTransformation
This document nees to be fixed.



## From [HansKindberg-Xml-Transformation](https://github.com/HansKindberg-Net/HansKindberg-Xml-Transformation)
# HansKindberg-Xml-Transformation
This is a solution for transforming xml-files/configuration-files on build by using the following **NuGet** functionality:

- [**Automatic import of msbuild targets and props files**](http://docs.nuget.org/docs/release-notes/nuget-2.5#Automatic_import_of_msbuild_targets_and_props_files)

## 1 HansKindberg-Xml-Transformation-On-Build
The pattern is:
- If *.Template.[fileextension] exists and *.$(Configuration).[fileextension] exists, *.Template.[fileextension] is transformed with *.$(Configuration).[fileextension] into *.[fileextension]
- If *.Template.[fileextension] exists but *.$(Configuration).[fileextension] does NOT exist, *.Template.[fileextension] is copied to *.[fileextension]

The default transformations are (see [**HansKindberg-Xml-Transformation-On-Build.props**](/HansKindberg-Xml-Transformation-On-Build/Build/HansKindberg-Xml-Transformation-On-Build.props)):

- App.Template.config / App.$(Configuration).config / App.config
- log4net.Template.config / log4net.$(Configuration).config / log4net.config
- Web.Template.config / Web.$(Configuration).config / Web.config
- Views\Web.Template.config / Views\Web.$(Configuration).config / Views\Web.config

You can change this behaviour by including/excluding items in the item-group **HansKindbergXmlTransformSource**.

### 1.1 Source control

- Do not include the *.[fileextension] in source-control.
- Include *.Template.[fileextension] and *.$(Configuration).[fileextension] in source-control.

### 1.2 Example

	<Content Include="Web.config" />
	<None Include="Web.Debug.config">
		<DependentUpon>Web.Template.config</DependentUpon>
	</None>
	<None Include="Web.Release.config">
		<DependentUpon>Web.Template.config</DependentUpon>
	</None>
	<None Include="Web.Template.config">
		<DependentUpon>Web.config</DependentUpon>
	</None>

## 2 HansKindberg-Xml-Transformation-Build-Files
[**1 HansKindberg-Xml-Transformation-On-Build**](#1-hanskindberg-xml-transformation-on-build) as content. I have laborated with referencing multiple **NuGet-Build**-packages in a Visual Studio project and have experienced problems when referencing. So if you want to include other **NuGet-Build** functionality together with **HansKindberg-Xml-Transformation-On-Build** the idea is to transform **HansKindberg-Xml-Transformation-Build-Files** into the package.



## From [Xml-Transformation-Lab-Temporary](https://github.com/HansKindberg-Net/Xml-Transformation-Lab-Temporary)
A project for laborating with XmlTransformation. Howto hookon and change the default behaviour.

I have set up a virtual machine where I can change the original targets to learn where things happen. I tried first to copy the original targets to this solution but there where to many paths to change to get it working.

The XmlTransformation targets is in (depending on the VS version):

C:\Program Files (x86)\MSBuild\Microsoft\VisualStudio\v12.0\Web\Microsoft.Web.Publishing.targets

	<PropertyGroup>
		<CollectWebConfigsToTransformDependsOn>
			$(OnBeforeCollectWebConfigsToTransform);
			$(CollectWebConfigsToTransformDependsOn);
			PipelineCollectFilesPhase;
		</CollectWebConfigsToTransformDependsOn>
	</PropertyGroup>

	<Target Name="CollectWebConfigsToTransform" DependsOnTargets="$(CollectWebConfigsToTransformDependsOn)" Condition="'$(CollectWebConfigsToTransform)' != 'false'">
		<!-- Gather Sources, Transforms, and Destinations for the TransformXml task -->
		<ItemGroup Condition="'$(ProjectConfigTransformFileName)'!=''">
			<WebConfigsToTransform Include="@(FilesForPackagingFromProject)" Condition="'%(FilesForPackagingFromProject.Filename)%(FilesForPackagingFromProject.Extension)'=='$(ProjectConfigFileName)'">
				<TransformFile>$([System.String]::new($(WebPublishPipelineProjectDirectory)\$([System.IO.Path]::GetDirectoryName($([System.String]::new(%(DestinationRelativePath)))))).TrimEnd('\'))\$(ProjectConfigTransformFileName)</TransformFile>
				<TransformOriginalFolder>$(TransformWebConfigIntermediateLocation)\original</TransformOriginalFolder>
				<TransformFileFolder>$(TransformWebConfigIntermediateLocation)\assist</TransformFileFolder>
				<TransformOutputFile>$(TransformWebConfigIntermediateLocation)\transformed\%(DestinationRelativePath)</TransformOutputFile>
				<TransformScope>$([System.IO.Path]::GetFullPath($(WPPAllFilesInSingleFolder)\%(DestinationRelativePath)))</TransformScope>
			</WebConfigsToTransform>
			<_WebConfigsToTransformOuputs Include="@(WebConfigsToTransform->'%(TransformOutputFile)')" />
		</ItemGroup>
		<CallTarget Targets="$(OnAfterCollectWebConfigsToTransform)" RunEachTargetSeparately="False" />
	</Target>

**Also have a look at the target: ParameterizeTransformWebConfigCore**

## Visual Studio Command Prompt

When I ran "MSBuild.exe" from the ordinary command prompt it opened up Visual Studio. So I tried the **Visual Studio Command Prompt** instead.

- [**Visual Studio Command Prompt**](http://msdn.microsoft.com/en-us/library/ms229859.aspx)

Run the **Visual Studio Command Prompt**:

- C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\Shortcuts\Developer Command Prompt for VS2013
- Right click -> Run as administrator (not sure if it is necessary, but...)
- CD "C:\Data\Projects\Xml-Transformation-Lab"

Examples:

- msbuild xml-transformation-lab.sln /p:DeployOnBuild=true;PublishProfile=Debug;AllowUntrustedCertificate=true

## Important

&lt;IsWebApplicationProject&gt; in WebApplication\Build\Build.targets is important to have to be able to have a package for both WebApplications and NONE WebApplication and make the
behaviour dependant on that: On build behaviour and On publish behaviour.

Look at, for $(Configuration) dependency:

- TransformWebConfigCore
- PreTransformWebConfig
- CollectWebConfigsToTransform

and look at, for Publish profile name .configs

- ProfileTransformWebConfigCore
- PreProfileTransformWebConfig
- CollectFilesForProfileTransformWebConfigs

Look here http://www.hanselman.com/blog/TinyHappyFeatures3PublishingImprovementsChainedConfigTransformsAndDeployingASPNETAppsFromTheCommandLine.aspx

**I can make a Web.config file with the same name as my Publish Profile and that transform will be run after the build transform.**

Lets say you have

	Web.config
		Web.Debug.config
		Web.Release.config
		Web.Production.config

and you have

- Debug
- Release

as configurations

and as Publish profile name you have

- Production

If the Publish profile **Production** is configured for the configuration **Release** the following happens when publishing

1. Transforming with Web.Release.config is done
2. Transforming with Web.Production.config is done