using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda;

namespace SkyrimRelationshipExtractor.Utilities
{
    internal static class MutagenResolvers
    {
        public static INpcGetter ResolveNpc(IGameEnvironment<ISkyrimMod, ISkyrimModGetter> env, IFormLinkGetter<ISkyrimMajorRecordGetter> npc)
        {
            return npc.Resolve<INpcGetter>(env.LinkCache);
        }

        public static IAssociationType ResolveAssociationType(IGameEnvironment<ISkyrimMod, ISkyrimModGetter> env, IFormLinkGetter<ISkyrimMajorRecordGetter> ass)
        {
            return ass.Resolve<IAssociationType>(env.LinkCache);
        }
    }
}
