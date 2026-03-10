public interface IVisitor
{
    void Visit(Player player);
    void Visit(Drone drone);
    void Visit(Turret turret);
}