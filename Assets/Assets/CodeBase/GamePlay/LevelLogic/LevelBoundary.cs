using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace SpaceShooter
{
    public class LevelBoundary : SingletonBase<LevelBoundary>
    {
        private Vector2 size = new Vector2(15f, 400f);  // Размер зоны (ширина и высота)

        private Vector2 sizeBorder = new Vector2(15f, 400f);
        private LineRenderer lineRenderer;

        public Vector2 SizeForLBLimiter => size;

        public enum Mode
        {
            Limit,
            Teleport,
            Death
        }

        [SerializeField] private Mode m_LimitMode;
        public Mode LimitMode => m_LimitMode;



        void Start()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();

            // Кол-во точек — 4: слева, снизу и справа линии,
            // без верхней линии
            lineRenderer.positionCount = 4;
            lineRenderer.loop = false;

            lineRenderer.widthMultiplier = 0.25f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;

            Vector3 halfSize = new Vector3(sizeBorder.x / 2, sizeBorder.y / 2, 0);

            Vector3[] points = new Vector3[4]
            {
            new Vector3(-halfSize.x, halfSize.y, 0), // левый верхний угол
            new Vector3(-halfSize.x, -2f, 0), // левый нижний угол
            new Vector3(halfSize.x, -2f, 0),  // правый нижний угол
            new Vector3(halfSize.x, halfSize.y, 0)   // правый верхний угол
           
            };

            lineRenderer.SetPositions(points);
        }






        //private void Start()
        //{
        //    lineRenderer = gameObject.AddComponent<LineRenderer>();
        //    lineRenderer.positionCount = 5; // 4 угла + замыкающая линия
        //    lineRenderer.loop = true;
        //    lineRenderer.widthMultiplier = 0.1f;
        //    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //    lineRenderer.startColor = Color.green;
        //    lineRenderer.endColor = Color.green;

        //    Vector3 center = transform.position;
        //    Vector3 halfSize = new Vector3(size.x / 2f, size.y / 2f, 0);

        //    Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y, 0);
        //    Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y, 0);
        //    Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y, 0);
        //    Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y, 0);

        //    lineRenderer.SetPosition(0, bottomLeft);
        //    lineRenderer.SetPosition(1, bottomRight);
        //    lineRenderer.SetPosition(2, topRight);
        //    lineRenderer.SetPosition(3, topLeft);
        //    lineRenderer.SetPosition(4, bottomLeft); // замыкаем контур
        //}
    }
}




//private Vector2 center = Vector2.zero; // Центр зоны
//[SerializeField] private SpaceShip ship;
//private Destructible destructible;



//private void Update()
//{
//    Vector2 pos = ship.transform.position;
//    Collider2D col = ship.GetComponent<Collider2D>();
//    Bounds bounds = col.bounds;

//    float left = center.x - size.x / 2f;
//    float right = center.x + size.x / 2f;
//    float bottom = center.y - size.y / 2f;



//    if (pos.x < left + bounds.size.x / 2 || pos.x > right - bounds.size.x / 2 || pos.y < bottom + bounds.size.y / 2)
//    {
//        if (destructible != null)
//        {
//            destructible.ApplyDamage(9999); // любое значение, чтобы точно убить
//        }

//    }
//}



//private void OnDrawGizmos()
//{
//    Vector3 center = transform.position;
//    Vector3 halfSize = new Vector3(size.x / 2f, size.y / 2f, 0);

//    Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y, 0);
//    Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y, 0);
//    Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y, 0);
//    Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y, 0);

//    Gizmos.color = Color.green;
//    Gizmos.DrawLine(bottomLeft, bottomRight);
//    Gizmos.DrawLine(bottomRight, topRight);
//    Gizmos.DrawLine(topRight, topLeft);
//    Gizmos.DrawLine(topLeft, bottomLeft);
//}







//private void OnDrawGizmosSelected()
//{

//Vector3 center = transform.position;
//Vector3 halfSize = new Vector3(size.x / 2f, size.y / 2f, 0);
//Эта строка вычисляет половину размера прямоугольника по 
//каждой оси(x и y), чтобы мы могли легко найти 
//координаты его углов относительно центра прямоугольника.

//            Почему "половина" ?
//Unity отрисовывает объекты(например, Gizmos или Handles) обычно от центра.
//                Поэтому, чтобы найти координаты всех 4 углов прямоугольника, 
//                нужно "отступить" от центра на половину ширины и высоты:

//                Слева: -size.x / 2

//                Справа: +size.x / 2

//                Снизу: -size.y / 2

//                Сверху: +size.y / 2









// Углы прямоугольника
//Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y, 0);
//Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y, 0);
//Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y, 0);
//Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y, 0);

//Handles.color = Color.green;

//// Рисуем три стороны: левая, нижняя, правая
//Handles.DrawLine(bottomLeft, bottomRight); // нижняя
//Handles.DrawLine(bottomLeft, topLeft);     // левая
//Handles.DrawLine(bottomRight, topRight);   // правая

//            Что рисуется:
//Левая сторона: от нижнего левого угла до верхнего левого.

//Нижняя сторона: от нижнего левого до нижнего правого.

//Правая сторона: от нижнего правого до верхнего правого.

//Верхняя не рисуется — прямоугольник "открыт сверху".