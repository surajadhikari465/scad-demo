# Schemas

The Schemas solution contains projects for auto-generating C# classes from XML schema definition (XSD) files. The C# classes can then be used to publish and consume messages from the TIBCO ESB.

## Code Generator projects

The Code Generator projects are WCF libraries which have built functionality to create C# classes from any XSDs inside of their project. We use these to simplify creating the C# classes for messaging so that we don't have to write them by hand.

## Schema Projects

The Schema Projects contain the generated C# classes. These projects are then turned into NuGet packages through our build process and used by other projects.

## Creating Schemas

Below are the steps to create or update C# classes with new XSDs. We'll use Icon.Esb.Schemas.Wfm as an example.

1. Create a CodeGenerator project for the group of XSDs if it doesn't already exist. This should be a WCF library to enable auto-generation of the C# classes.
2. Create a Schemas folder inside of the CodeGenerator project to house the XSDs.
3. Copy XSDs to the Schemas folder. Make sure they are included in the project.
4. Build the project
5. Click "Show All Files" in the Solution Explorer to view all hidden files under the CodeGenerator project
6. Navigate under your CodeGenerator project to obj\Debug\TempPE\XsdGeneratedCode\GeneratedXsdTypes.cs. The GeneratedXsdTypes.cs file contains all of the generated C# classes from the XSD.
7. Create a project that will house the output C# classes if it does not already exist. This should be a .NET Class Library.
8. Create a GeneratedCode folder under the new project.
9. Copy the GeneratedXsdTypes.cs from the CodeGenerator project to the GeneratedCode folder in the output project and rename the file to something descriptive. For example we named the file to "WfmContracts.cs" for Icon.Esb.Schemas.Wfm.
10. Update the namespace of the copied GeneratedXsdTypes.cs to match the output project namespace. For example, Icon.Esb.Schemas.Wfm uses Icon.Esb.Schemas.Wfm.Contracts as it's namespace.
11. You're done! The schema project should contain the new/updated C# classes that match the XSDs from the CodeGenerator project. Push the changes to source control and run a build to create an updated NuGet package.

## Creating Custom Types in Schema Projects

We've run into an issue where auto generation of the schemas results in bad types or not generating some types. In order to resolve this you may need to adjust the auto-generated C# classes by hand or create your own type and add them to the project. This should be done seldomly and should be resolved by updating the XSDs themselves. But in order to save time especially if the XSDs are from other teams you may need to create these custom types manually. Any updates like these should be written in the README.md of the project themselves so that other devs are aware.