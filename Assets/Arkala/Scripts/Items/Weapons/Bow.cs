using ClickNext.Scripts.Units.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace ClickNext.Scripts.Items.Weapons
{
    [DisallowMultipleComponent]
    public class Bow : Weapon
    {
        [SerializeField] GameObject spawnProjectile;
        [SerializeField] float projectileSpeed;

        private GameObject projectile;

        private void Awake() => spawnProjectile.gameObject.SetActive(false);

        public override IEnumerator DelayAttack(Unit target, Action onAttack, Action<float> onHit)
        {
            onAttack();

            var archeryTarget = target.archeryTargets.Length == 0 ? target.transform :
                target.archeryTargets[UnityEngine.Random.Range(0, target.archeryTargets.Length)];

            yield return new WaitForSeconds(_animationDelay);

            projectile = SpawnProjectile();

            //makesure arrow hit the target.
            float maxTime = 2f;
            var distance = float.MaxValue;
            while (distance > 1f)
            {
                yield return null;
                if (archeryTarget && archeryTarget.gameObject.activeInHierarchy)
                {
                    distance = Vector3.Distance(projectile.transform.position, archeryTarget.position);
                    projectile.gameObject.SetActive(true);
                    projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, archeryTarget.position, projectileSpeed * Time.deltaTime);
                    projectile.transform.LookAt(archeryTarget);
                }
                else
                {
                    Destroy(projectile);
                    projectile = null;
                    yield break;
                }

                maxTime -= Time.deltaTime;
                if (maxTime <= 0)
                    break;
            }

            if (target && target.gameObject.activeInHierarchy)
            {
                projectile.transform.SetParent(target.transform);
                projectile = null;
            }

            onHit(_damage);
        }

        private void OnDisable()
        {
            if (projectile)
                Destroy(projectile);
        }

        private GameObject SpawnProjectile()
        {
            var projectile = Instantiate(spawnProjectile);
            projectile.transform.position = spawnProjectile.transform.position;
            projectile.transform.localScale = spawnProjectile.transform.lossyScale;
            return projectile;
        }
    }
}