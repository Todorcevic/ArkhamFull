using System.Collections;

namespace ArkhamGamePlay
{
    public delegate IEnumerator Effect();
    public delegate IEnumerator EffectWithCard(CardComponent card);
}