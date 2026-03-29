using System;
using System.Reflection;
using HarmonyLib;
using SmithForever.Patches;
using Xunit;

namespace SmithForever.Tests
{
    /// <summary>
    /// Tests for <see cref="RefiningStaminaPatch"/>, <see cref="SmithingStaminaPatch"/>,
    /// and <see cref="SmeltingStaminaPatch"/> Harmony prefix methods and attributes.
    /// </summary>
    public class SmithingStaminaPatchTests
    {
        // ── RefiningStaminaPatch ─────────────────────────────────────────

        [Fact]
        public void RefiningPrefix_SetsResultToZero()
        {
            int result = 999;
            bool executeOriginal = RefiningStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void RefiningPrefix_SetsResultToZero_WhenAlreadyZero()
        {
            int result = 0;
            bool executeOriginal = RefiningStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void RefiningPrefix_SetsResultToZero_WhenNegative()
        {
            int result = -5;
            bool executeOriginal = RefiningStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void RefiningPatch_HasCorrectHarmonyPatchAttribute()
        {
            var attr = typeof(RefiningStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr);
        }

        [Fact]
        public void RefiningPatch_TargetsDefaultSmithingModel()
        {
            var attrs = typeof(RefiningStaminaPatch).GetCustomAttributes<HarmonyPatch>();
            var attr = Assert.Single(attrs);
            Assert.Equal(typeof(TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel), attr.info.declaringType);
        }

        [Fact]
        public void RefiningPatch_TargetsGetEnergyCostForRefiningMethod()
        {
            var attr = typeof(RefiningStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.Equal("GetEnergyCostForRefining", attr.info.methodName);
        }

        [Fact]
        public void RefiningPatch_SpecifiesCorrectArgumentTypes()
        {
            var attr = typeof(RefiningStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr.info.argumentTypes);
            Assert.Equal(2, attr.info.argumentTypes.Length);
            // First argument is marked ArgumentType.Ref, so it's stored as a by-ref type
            Assert.Equal(typeof(TaleWorlds.Core.Crafting.RefiningFormula).MakeByRefType(), attr.info.argumentTypes[0]);
            Assert.Equal(typeof(TaleWorlds.CampaignSystem.Hero), attr.info.argumentTypes[1]);
        }

        [Fact]
        public void RefiningPatch_UsesRefArgumentVariationForFormula()
        {
            // Verify the [HarmonyPatch] attribute constructor was called with ArgumentType arrays.
            // The attribute on RefiningStaminaPatch specifies ArgumentType.Ref for the first param
            // and ArgumentType.Normal for the second. We verify this through the CustomAttributeData
            // to access the raw constructor arguments.
            var attrData = System.Linq.Enumerable.FirstOrDefault(
                typeof(RefiningStaminaPatch).GetCustomAttributesData(),
                a => a.AttributeType == typeof(HarmonyPatch));

            Assert.NotNull(attrData);
            // The 4th constructor argument is the ArgumentType[] array
            Assert.True(attrData.ConstructorArguments.Count >= 4,
                "HarmonyPatch attribute should have at least 4 constructor arguments.");

            var argVariations = (System.Collections.ObjectModel.ReadOnlyCollection<System.Reflection.CustomAttributeTypedArgument>)
                attrData.ConstructorArguments[3].Value;
            Assert.Equal(2, argVariations.Count);
            Assert.Equal((int)ArgumentType.Ref, (int)argVariations[0].Value);
            Assert.Equal((int)ArgumentType.Normal, (int)argVariations[1].Value);
        }

        [Fact]
        public void RefiningPatch_ClassIsInternalStatic()
        {
            var type = typeof(RefiningStaminaPatch);
            Assert.True(type.IsAbstract && type.IsSealed, "Class should be static (abstract + sealed).");
            Assert.False(type.IsPublic, "Patch class should be internal, not public.");
        }

        // ── SmithingStaminaPatch ─────────────────────────────────────────

        [Fact]
        public void SmithingPrefix_SetsResultToZero()
        {
            int result = 150;
            bool executeOriginal = Patches.SmithingStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void SmithingPrefix_SetsResultToZero_WhenAlreadyZero()
        {
            int result = 0;
            bool executeOriginal = Patches.SmithingStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void SmithingPrefix_SetsResultToZero_WhenNegative()
        {
            int result = -10;
            bool executeOriginal = Patches.SmithingStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void SmithingPatch_HasCorrectHarmonyPatchAttribute()
        {
            var attr = typeof(Patches.SmithingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr);
        }

        [Fact]
        public void SmithingPatch_TargetsDefaultSmithingModel()
        {
            var attr = typeof(Patches.SmithingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.Equal(typeof(TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel), attr.info.declaringType);
        }

        [Fact]
        public void SmithingPatch_TargetsGetEnergyCostForSmithingMethod()
        {
            var attr = typeof(Patches.SmithingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.Equal("GetEnergyCostForSmithing", attr.info.methodName);
        }

        [Fact]
        public void SmithingPatch_SpecifiesCorrectArgumentTypes()
        {
            var attr = typeof(Patches.SmithingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr.info.argumentTypes);
            Assert.Equal(2, attr.info.argumentTypes.Length);
            Assert.Equal(typeof(TaleWorlds.Core.ItemObject), attr.info.argumentTypes[0]);
            Assert.Equal(typeof(TaleWorlds.CampaignSystem.Hero), attr.info.argumentTypes[1]);
        }

        [Fact]
        public void SmithingPatch_ClassIsInternalStatic()
        {
            var type = typeof(Patches.SmithingStaminaPatch);
            Assert.True(type.IsAbstract && type.IsSealed, "Class should be static (abstract + sealed).");
            Assert.False(type.IsPublic, "Patch class should be internal, not public.");
        }

        // ── SmeltingStaminaPatch ─────────────────────────────────────────

        [Fact]
        public void SmeltingPrefix_SetsResultToZero()
        {
            int result = 75;
            bool executeOriginal = SmeltingStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void SmeltingPrefix_SetsResultToZero_WhenAlreadyZero()
        {
            int result = 0;
            bool executeOriginal = SmeltingStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void SmeltingPrefix_SetsResultToZero_WhenNegative()
        {
            int result = -3;
            bool executeOriginal = SmeltingStaminaPatch.Prefix(ref result);

            Assert.Equal(0, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void SmeltingPatch_HasCorrectHarmonyPatchAttribute()
        {
            var attr = typeof(SmeltingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr);
        }

        [Fact]
        public void SmeltingPatch_TargetsDefaultSmithingModel()
        {
            var attr = typeof(SmeltingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.Equal(typeof(TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel), attr.info.declaringType);
        }

        [Fact]
        public void SmeltingPatch_TargetsGetEnergyCostForSmeltingMethod()
        {
            var attr = typeof(SmeltingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.Equal("GetEnergyCostForSmelting", attr.info.methodName);
        }

        [Fact]
        public void SmeltingPatch_SpecifiesCorrectArgumentTypes()
        {
            var attr = typeof(SmeltingStaminaPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr.info.argumentTypes);
            Assert.Equal(2, attr.info.argumentTypes.Length);
            Assert.Equal(typeof(TaleWorlds.Core.ItemObject), attr.info.argumentTypes[0]);
            Assert.Equal(typeof(TaleWorlds.CampaignSystem.Hero), attr.info.argumentTypes[1]);
        }

        [Fact]
        public void SmeltingPatch_ClassIsInternalStatic()
        {
            var type = typeof(SmeltingStaminaPatch);
            Assert.True(type.IsAbstract && type.IsSealed, "Class should be static (abstract + sealed).");
            Assert.False(type.IsPublic, "Patch class should be internal, not public.");
        }

        // ── Cross-cutting: all patches in same namespace ─────────────────

        [Fact]
        public void AllPatchClasses_AreInPatchesNamespace()
        {
            Assert.Equal("SmithForever.Patches", typeof(RefiningStaminaPatch).Namespace);
            Assert.Equal("SmithForever.Patches", typeof(Patches.SmithingStaminaPatch).Namespace);
            Assert.Equal("SmithForever.Patches", typeof(SmeltingStaminaPatch).Namespace);
        }

        [Fact]
        public void AllPatchPrefixes_HaveConsistentSignature()
        {
            // All three Prefix methods should have identical signatures: static bool Prefix(ref int __result)
            var prefixMethods = new[]
            {
                typeof(RefiningStaminaPatch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic),
                typeof(Patches.SmithingStaminaPatch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic),
                typeof(SmeltingStaminaPatch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic),
            };

            foreach (var method in prefixMethods)
            {
                Assert.NotNull(method);
                Assert.Equal(typeof(bool), method.ReturnType);

                var parameters = method.GetParameters();
                Assert.Single(parameters);
                Assert.Equal("__result", parameters[0].Name);
                Assert.True(parameters[0].ParameterType.IsByRef, "Parameter should be passed by ref.");
                Assert.Equal(typeof(int), parameters[0].ParameterType.GetElementType());
            }
        }

        [Fact]
        public void AllPatches_TargetSameDeclaringType()
        {
            var expectedType = typeof(TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel);

            var types = new[]
            {
                typeof(RefiningStaminaPatch),
                typeof(Patches.SmithingStaminaPatch),
                typeof(SmeltingStaminaPatch),
            };

            foreach (var patchType in types)
            {
                var attr = patchType.GetCustomAttribute<HarmonyPatch>();
                Assert.Equal(expectedType, attr.info.declaringType);
            }
        }

        [Fact]
        public void AllPatches_TargetDistinctMethods()
        {
            var types = new[]
            {
                typeof(RefiningStaminaPatch),
                typeof(Patches.SmithingStaminaPatch),
                typeof(SmeltingStaminaPatch),
            };

            var methodNames = new System.Collections.Generic.HashSet<string>();
            foreach (var patchType in types)
            {
                var attr = patchType.GetCustomAttribute<HarmonyPatch>();
                Assert.True(methodNames.Add(attr.info.methodName),
                    $"Duplicate target method: {attr.info.methodName}");
            }

            Assert.Equal(3, methodNames.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void AllPrefixes_AlwaysReturnZeroAndFalse(int initialValue)
        {
            int r1 = initialValue;
            Assert.False(RefiningStaminaPatch.Prefix(ref r1));
            Assert.Equal(0, r1);

            int r2 = initialValue;
            Assert.False(Patches.SmithingStaminaPatch.Prefix(ref r2));
            Assert.Equal(0, r2);

            int r3 = initialValue;
            Assert.False(SmeltingStaminaPatch.Prefix(ref r3));
            Assert.Equal(0, r3);
        }
    }
}
