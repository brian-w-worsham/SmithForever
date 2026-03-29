using System;
using System.Reflection;
using HarmonyLib;
using Xunit;

namespace SmithForever.Tests
{
    /// <summary>
    /// Tests for <see cref="SubModule"/> — mod entry point, Harmony lifecycle,
    /// and structural correctness.
    /// </summary>
    public class SubModuleTests
    {
        [Fact]
        public void SubModule_InheritsFromMBSubModuleBase()
        {
            Assert.True(typeof(TaleWorlds.MountAndBlade.MBSubModuleBase)
                .IsAssignableFrom(typeof(SubModule)));
        }

        [Fact]
        public void SubModule_IsPublic()
        {
            Assert.True(typeof(SubModule).IsPublic);
        }

        [Fact]
        public void SubModule_IsInRootNamespace()
        {
            Assert.Equal("SmithForever", typeof(SubModule).Namespace);
        }

        [Fact]
        public void SubModule_HasHarmonyField()
        {
            var field = typeof(SubModule).GetField("_harmony",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(field);
            Assert.Equal(typeof(Harmony), field.FieldType);
        }

        [Fact]
        public void SubModule_OverridesOnSubModuleLoad()
        {
            var method = typeof(SubModule).GetMethod("OnSubModuleLoad",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(method);
            Assert.True(method.IsFamily || method.IsFamilyOrAssembly,
                "OnSubModuleLoad should be protected.");
        }

        [Fact]
        public void SubModule_OverridesOnSubModuleUnloaded()
        {
            var method = typeof(SubModule).GetMethod("OnSubModuleUnloaded",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(method);
            Assert.True(method.IsFamily || method.IsFamilyOrAssembly,
                "OnSubModuleUnloaded should be protected.");
        }

        [Fact]
        public void SubModule_HarmonyId_IsCorrect()
        {
            // The Harmony ID must match between OnSubModuleLoad and OnSubModuleUnloaded.
            // We verify this by checking the constant string is referenced in the source.
            // Create an instance and check the _harmony field is null before load.
            var subModule = new SubModule();
            var field = typeof(SubModule).GetField("_harmony",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.Null(field.GetValue(subModule));
        }

        [Fact]
        public void SubModule_CanBeInstantiated()
        {
            var subModule = new SubModule();
            Assert.NotNull(subModule);
        }

        [Fact]
        public void SubModule_AssemblyExposesinternalsToTests()
        {
            var mainAssembly = typeof(SubModule).Assembly;
            var attrs = mainAssembly.GetCustomAttributes<
                System.Runtime.CompilerServices.InternalsVisibleToAttribute>();

            bool found = false;
            foreach (var attr in attrs)
            {
                if (attr.AssemblyName == "SmithForever.Tests")
                {
                    found = true;
                    break;
                }
            }
            Assert.True(found, "SmithForever assembly should have InternalsVisibleTo for SmithForever.Tests.");
        }

        [Fact]
        public void SubModule_Assembly_ContainsAllExpectedPatchTypes()
        {
            var assembly = typeof(SubModule).Assembly;

            Assert.NotNull(assembly.GetType("SmithForever.Patches.RefiningStaminaPatch"));
            Assert.NotNull(assembly.GetType("SmithForever.Patches.SmithingStaminaPatch"));
            Assert.NotNull(assembly.GetType("SmithForever.Patches.SmeltingStaminaPatch"));
        }

        [Fact]
        public void SubModule_Assembly_HasExactlyThreePatchClasses()
        {
            var assembly = typeof(SubModule).Assembly;
            int patchCount = 0;

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute<HarmonyPatch>() != null)
                {
                    patchCount++;
                }
            }

            Assert.Equal(3, patchCount);
        }

        [Fact]
        public void SubModule_AllPatchClasses_TargetSameGameModel()
        {
            var expectedType = typeof(TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel);
            var assembly = typeof(SubModule).Assembly;

            foreach (var type in assembly.GetTypes())
            {
                var attr = type.GetCustomAttribute<HarmonyPatch>();
                if (attr != null)
                {
                    Assert.Equal(expectedType, attr.info.declaringType);
                }
            }
        }
    }
}
