namespace EternalReturn.Stress.Collision_System
{
    public interface IColliding
    {
        Transform2D Transform { get; }

        Mesh2D Mesh { get; }

        void OnCollisionEnter(IColliding other);
        void OnCollisionStay(IColliding other);
        void OnCollisionExit(IColliding other);
    }
}
