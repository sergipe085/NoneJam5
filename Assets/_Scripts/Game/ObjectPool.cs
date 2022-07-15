using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private List<Bullet> bulletsPoolList = new();
    [SerializeField] private int maxBullets = 10;

    public Bullet SpawnBullet2(Vector2 position) {
        Bullet bullet;

        if (bulletsPoolList.Count <= 0 || bulletsPoolList.Count < maxBullets) {
            Bullet newBullet = Instantiate(bulletPrefab);
            bulletsPoolList.Add(newBullet);
            bullet = newBullet;
        }
        else {
            bullet = bulletsPoolList[0];
            bulletsPoolList.Remove(bullet);
            bulletsPoolList.Add(bullet);
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
