namespace MTCG.Models.Components
{
    public abstract class Component 
    {
        public abstract Component deserializeFromJsonObject(dynamic jsonObject);
    }
}
