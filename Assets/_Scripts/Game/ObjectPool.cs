using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Queue<Bullet> spawnedBullets = new();
    [SerializeField] private Queue<Bullet> bulletsPool = new();
    [SerializeField] private int maxBullets = 10;

    public Bullet SpawnBullet(Vector2 position) {
        Bullet bullet;
        
        if (bulletsPool.Count <= 0) {
            if (spawnedBullets.Count < maxBullets) {
                Bullet newBullet = Instantiate(bulletPrefab);
                newBullet.gameObject.SetActive(false);
                bulletsPool.Enqueue(newBullet);
            }
            else {
                Bullet bulletToDestroy = spawnedBullets.Dequeue();
                DestroyBullet(bulletToDestroy);
            }
        }

        bullet = bulletsPool.Dequeue();
        spawnedBullets.Enqueue(bullet);
        bullet.gameObject.SetActive(true);
        bullet.transform.position = position;
        return bullet;
    }

    public void DestroyBullet(Bullet bullet) {
        bullet.gameObject.SetActive(false);
        bullet.transform.position = Vector2.zero;
        bulletsPool.Enqueue(bullet);
    }
}
