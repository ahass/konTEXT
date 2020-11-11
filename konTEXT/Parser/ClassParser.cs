using System.Linq;
using konTEXT.Models;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace konTEXT.Parser
{
    public static class ClassParser
    {
        public static ClassModel Parse(string fileContent)
        {
            if (fileContent.Equals(string.Empty)) return new ClassModel();

            var syntaxTree = CSharpSyntaxTree.ParseText(fileContent);

            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            var namespaceSyntax = root?.Members.OfType<NamespaceDeclarationSyntax>().First();

            var classModel = new ClassModel();

            // Todo current limitation, only one class in file
            var classSyntax = namespaceSyntax?.Members.OfType<ClassDeclarationSyntax>().First(); // Todo error on apply to interface

            var methodSyntax = classSyntax?.Members.OfType<MethodDeclarationSyntax>();

            if (classSyntax != null)
            {
                var propertySyntax = classSyntax.Members.OfType<PropertyDeclarationSyntax>();

                var fieldSyntax = classSyntax.Members.OfType<FieldDeclarationSyntax>();

                var constructorSyntax = classSyntax.Members.OfType<ConstructorDeclarationSyntax>();

                var eventSyntax = classSyntax.Members.OfType<EventDeclarationSyntax>();

                var enumSyntax = classSyntax.Members.OfType<EnumDeclarationSyntax>();
                
                classModel.NamespaceDeclarationSyntax = namespaceSyntax;
                classModel.ClassDeclarationSyntax = classSyntax;
                classModel.MethodDeclarationSyntax = methodSyntax;
                classModel.PropertyDeclarationSyntax = propertySyntax;
                classModel.FieldDeclarationSyntax = fieldSyntax;
                classModel.ConstructorDeclarationSyntax = constructorSyntax;
                classModel.EventDeclarationSyntax = eventSyntax;
                classModel.EnumDeclarationSyntax = enumSyntax;

            }

            return classModel;
        }
    }
}
