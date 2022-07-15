using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Queue<Bullet> spawnedBullets = new();
    [SerializeField] private Stack<Bullet> bulletsPool = new();
    [SerializeField] private int maxBullets = 10;

    public Bullet SpawnBullet2(Vector2 position) {
        Bullet bullet;

        if (bulletsPool.Count <= 0 || bulletsPool.Count < maxBullets) {
            Bullet newBullet = Instantiate(bulletPrefab);
            bulletsPool.Push(newBullet);
            bullet = newBullet;
        }
        else {
            bullet = bulletsPool.Peek();
        }

        bullet.gameObject.SetActive(true);
        bullet.transform.position = position;
        return bullet;
    }

    public void DestroyBullet2(Bullet bullet) {
        bullet.gameObject.SetActive(false);
        bullet.transform.position = Vector2.zero;
    }
}
