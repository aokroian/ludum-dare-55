namespace _Game.Scripts.CharacterRelated.Actors.Upgrades
{
    public interface IActorStatsReceiver
    {
        public void RegisterActorStatsReceiver();
        public void UnregisterActorStatsReceiver();
        public void ReceiveActorStats(ActorStatsSo actorStatsSo);
    }
}