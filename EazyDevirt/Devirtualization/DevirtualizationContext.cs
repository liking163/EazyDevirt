﻿using AsmResolver.DotNet;
using AsmResolver.DotNet.Serialized;
using AsmResolver.PE.DotNet.Metadata.Tables;
using EazyDevirt.Architecture;
using EazyDevirt.Core.IO;
using EazyDevirt.Devirtualization.Options;
using EazyDevirt.Logging;
using EazyDevirt.PatternMatching;

namespace EazyDevirt.Devirtualization;

internal record DevirtualizationContext
{
    // got lazy
    public static DevirtualizationContext Instance { get; set; } = null!;
    
    public DevirtualizationOptions Options { get; }
    public ModuleDefinition Module { get; }
    public PatternMatcher PatternMatcher { get; }
    public ConsoleLogger Console { get; }
    public ReferenceImporter Importer { get; }
    
    public MetadataToken VMResourceGetterMdToken { get; set; }
    
    
    /// <summary>
    /// VM method locals field.
    /// Used in pattern matching ldloc and stloc opcodes.
    /// </summary>
    public FieldDefinition VMLocalsField { get; set; }

    /// <summary>
    /// VM method arguments field.
    /// Used in pattern matching ldarg and starg opcodes.
    /// </summary>
    public FieldDefinition VMArgumentsField { get; set; }
    public MethodDefinition VMExecuteVMMethod { get; set; }
    public TypeDefinition VMDeclaringType { get; set; }
    public VMCipherStream VMStream { get; set; }
    public VMCipherStream VMResolverStream { get; set; }
    public int PositionCryptoKey { get; set; }
    public int MethodCryptoKey { get; set; }

    public List<VMMethod> VMMethods { get; set; }

    public DevirtualizationContext(DevirtualizationOptions opts)
    {
        Options = opts;
        Module = ModuleDefinition.FromFile(Options.Assembly.FullName);
        PatternMatcher = new PatternMatcher();
        Console = new ConsoleLogger();
        Importer = new ReferenceImporter(Module);
    }
}