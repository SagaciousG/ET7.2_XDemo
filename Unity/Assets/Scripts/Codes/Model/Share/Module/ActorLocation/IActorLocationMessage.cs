namespace ET
{
    public interface IActorLocationMessage: IActorRequest
    {
    }

    public interface IActorLocationRequest: IActorRequest
    {
    }

    public interface IActorLocationResponse: IActorResponse
    {
    }

    public interface IUnitMessage: IActorLocationMessage
    {
        
    }

    public interface IUnitRequest: IActorLocationRequest
            
    {
        
    }

    public interface IUnitResponse: IActorLocationResponse
    {
        
    }

    public interface IBattleMessage: IUnitMessage
    {
        
    }

    public interface IBattleRequest: IUnitRequest
    {
        
    }

    public interface IBattleResponse: IUnitResponse
    {
        
    }
    
}