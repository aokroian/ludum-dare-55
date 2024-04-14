using Actors;
using Actors.ActorSystems;
using Actors.Combat;
using Actors.InputThings;
using UI.Hud;
using UnityEngine;

namespace Bonus.BonusTypes
{
    public class RifleBonus : AbstractBonus
    {
        [SerializeField] private int shotsAmount = 10;
        
        private int _shotsLeft;
        private ActorGunSystem _playerGunSystem;
        private PlayerWeaponHud _weaponHud;

        protected override void ApplyBonus(PlayerActorInput player)
        {
            _playerGunSystem = player.GetComponent<ActorGunSystem>();
            var gun = _playerGunSystem.ChangeActiveGun(GunTypes.Shotgun);
            gun.OnFire += OnFire;
            _shotsLeft = shotsAmount;
            _weaponHud = player.GetComponentInChildren<PlayerWeaponHud>();
            _weaponHud.SetShots(_shotsLeft, shotsAmount);
        }
        
        private void OnFire()
        {
            _shotsLeft--;
            _weaponHud.SetShots(_shotsLeft, shotsAmount);
            if (_shotsLeft <= 0)
            {
                _playerGunSystem.ChangeActiveGun(GunTypes.Pistol);
                Destroy(gameObject);
            }
        }
    }
}