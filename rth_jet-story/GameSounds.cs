using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace rth_jet_story
{
    class GameSounds
    {
        private static GameSounds self = null;

        public static GameSounds getInstance()
        {
            if (self == null)
            {
                self = new GameSounds();
            }
            return self;
        }

        public enum SoundsIDs
        {
            SOUND_BOOM,
            SOUND_BASE_BOOM,
            SOUND_CANON,
            SOUND_ITEM_GET,
            SOUND_ETYPE_1_SHOT,
            SOUND_ETYPE_3_13_SHOT,
            SOUND_ETYPE_2_SHOT,
            SOUND_ETYPE_5_SHOT,
            SOUND_ETYPE_6_SHOT,
            SOUND_ETYPE_7_SHOT,
            SOUND_ETYPE_8_SHOT,
            SOUND_ETYPE_9_LAUNCH,
            SOUND_ETYPE_9_SHOT,
            SOUND_ETYPE_10,
            SOUND_DAMAGE,
            SOUND_SHIP_DAMAGE,
            SOUND_ENEMY_DAMAGE,
            SOUND_CIRLCE_BOUNCE,
            SOUND_SPECIAL_LAUNCH
        };

        bool enabled;

        private SoundEffect boom;
        private SoundEffect cannon;
        private SoundEffect baseBoom;
        private SoundEffect itemGet;

        private SoundEffect e1s_shot; //continuous
        private SoundEffectInstance e1s_shot_I = null;
        private SoundEffect e2s_shot;
        private SoundEffect e3s_13s_shot;
        private SoundEffect e5s_shot; //continuous
        private SoundEffectInstance e5s_shot_I = null;
        private SoundEffect e6s_shot; //continuous
        private SoundEffectInstance e6s_shot_I = null;
        private SoundEffect e7s_shot;
        private SoundEffect e8s_shot; //cont.
        private SoundEffectInstance e8s_shot_I = null;
        private SoundEffect e9s_launch;
        private SoundEffect e9s_shot; //cont.
        private SoundEffectInstance e9s_shot_I = null;
        private SoundEffect e10s_shot;

        private SoundEffect shipDmg;
        private SoundEffect enemyDmg;
        private SoundEffect damageSound; //cont.
        private SoundEffectInstance damageSound_I = null;
        bool damageSoundIsPlaying;

        private SoundEffect circleBounce;
        private SoundEffect specialLaunch;

        private List<SoundEffectInstance> playingSounds;

        public void loadSounds(ContentManager content)
        {
            this.boom = content.Load<SoundEffect>("sounds/boom");
            this.cannon = content.Load<SoundEffect>("sounds/ship_cannon_shot");
            this.baseBoom = content.Load<SoundEffect>("sounds/boom_base");
            this.itemGet = content.Load<SoundEffect>("sounds/get_item");

            this.e1s_shot = content.Load<SoundEffect>("sounds/enemy_1_shot");
            this.e2s_shot = content.Load<SoundEffect>("sounds/enemy_2_shot");
            this.e3s_13s_shot = content.Load<SoundEffect>("sounds/enemy_3_13_shot");
            this.e5s_shot = content.Load<SoundEffect>("sounds/enemy_5_shot");
            this.e6s_shot = content.Load<SoundEffect>("sounds/enemy_6_shot");
            this.e7s_shot = content.Load<SoundEffect>("sounds/enemy_7_shot");
            this.e8s_shot = content.Load<SoundEffect>("sounds/enemy_8_shot");
            this.e9s_launch = content.Load<SoundEffect>("sounds/enemy_9_launch");
            this.e9s_shot = content.Load<SoundEffect>("sounds/enemy_9_shot");
            this.e10s_shot = content.Load<SoundEffect>("sounds/enemy_10");

            this.shipDmg = content.Load<SoundEffect>("sounds/ship_damage");
            this.enemyDmg = content.Load<SoundEffect>("sounds/enemy_damage");
            this.damageSound = content.Load<SoundEffect>("sounds/damage");
            
            this.circleBounce = content.Load<SoundEffect>("sounds/circle_bounce");
            this.specialLaunch = content.Load<SoundEffect>("sounds/special_launch");
        }

        public void disposeSounds()
        {
            boom.Dispose();
            cannon.Dispose();
            baseBoom.Dispose();
            itemGet.Dispose();

            e1s_shot.Dispose(); 
            e2s_shot.Dispose();
            e3s_13s_shot.Dispose();
            e5s_shot.Dispose(); 
            e6s_shot.Dispose();
            e7s_shot.Dispose();
            e8s_shot.Dispose();
            e9s_launch.Dispose();
            e9s_shot.Dispose();
            e10s_shot.Dispose();

            shipDmg.Dispose();
            enemyDmg.Dispose();
            damageSound.Dispose();

            circleBounce.Dispose();
            specialLaunch.Dispose();
        }

        private GameSounds()
        {
            this.enabled = true;

            this.playingSounds = new List<SoundEffectInstance>();
        }

        public void playSound(SoundsIDs id)
        {
            if (this.enabled)
            {
                switch (id)
                {
                    case SoundsIDs.SOUND_BOOM: this.boom.Play(); break;

                    case SoundsIDs.SOUND_CANON: this.cannon.Play(); break;

                    case SoundsIDs.SOUND_BASE_BOOM: this.baseBoom.Play(); break;

                    case SoundsIDs.SOUND_ITEM_GET: this.itemGet.Play(); break;

                    case SoundsIDs.SOUND_ETYPE_1_SHOT:
                        if (this.e1s_shot_I != null)
                        {
                            this.e1s_shot_I.Stop();
                        }
                        //this.e1s_shot_I = this.e1s_shot.Play(0.5f, 0.0f, 0.0f, true);
                        this.e1s_shot.Play(0.5f, 0.0f, 0.0f);
                        this.playingSounds.Add(this.e1s_shot_I);
                        break;

                    case SoundsIDs.SOUND_ETYPE_2_SHOT: this.e2s_shot.Play(); break;

                    case SoundsIDs.SOUND_ETYPE_3_13_SHOT: this.e3s_13s_shot.Play(); break;

                    case SoundsIDs.SOUND_ETYPE_5_SHOT:
                        if (this.e5s_shot_I != null)
                        {
                            this.e5s_shot_I.Stop();
                        }
                        //this.e5s_shot_I = this.e5s_shot.Play(0.5f, 0.0f, 0.0f, true);
                        this.e5s_shot.Play(0.5f, 0.0f, 0.0f);
                        this.playingSounds.Add(this.e5s_shot_I);
                        break;

                    case SoundsIDs.SOUND_ETYPE_6_SHOT:
                        if (this.e6s_shot_I != null)
                        {
                            this.e6s_shot_I.Stop();
                        }
                        //this.e6s_shot_I = this.e6s_shot.Play(0.5f, 0.0f, 0.0f, true);
                        this.e6s_shot.Play(0.5f, 0.0f, 0.0f);
                        this.playingSounds.Add(this.e6s_shot_I);
                        break;

                    case SoundsIDs.SOUND_ETYPE_7_SHOT: this.e7s_shot.Play(); break;

                    case SoundsIDs.SOUND_ETYPE_8_SHOT:
                        if (this.e8s_shot_I != null)
                        {
                            this.e8s_shot_I.Stop();
                        }
                        //this.e8s_shot_I = this.e8s_shot.Play(0.5f, 0.0f, 0.0f, true);
                        this.e8s_shot.Play(0.5f, 0.0f, 0.0f);
                        this.playingSounds.Add(this.e8s_shot_I);
                        break;

                    case SoundsIDs.SOUND_ETYPE_9_LAUNCH: this.e9s_launch.Play(); break;

                    case SoundsIDs.SOUND_ETYPE_9_SHOT:
                        if (this.e9s_shot_I != null)
                        {
                            this.e9s_shot_I.Stop();
                        }
                        //this.e9s_shot_I = this.e9s_shot.Play(0.5f, 0.0f, 0.0f, true);
                        this.e9s_shot.Play(0.5f, 0.0f, 0.0f);
                        this.playingSounds.Add(this.e9s_shot_I);
                        break;

                    case SoundsIDs.SOUND_ETYPE_10: this.e10s_shot.Play(); break;

                    case SoundsIDs.SOUND_SHIP_DAMAGE: this.shipDmg.Play(); break;

                    case SoundsIDs.SOUND_ENEMY_DAMAGE: this.enemyDmg.Play(); break;

                    case SoundsIDs.SOUND_DAMAGE:
                        if (!this.damageSoundIsPlaying)
                        {
                            //this.damageSound_I = this.damageSound.Play(0.5f, 0.0f, 0.0f, true);
                            this.damageSound.Play(0.5f, 0.0f, 0.0f);
                            this.playingSounds.Add(this.damageSound_I);
                            this.damageSoundIsPlaying = true;
                        }
                        break;

                    case SoundsIDs.SOUND_CIRLCE_BOUNCE: this.circleBounce.Play(); break;

                    case SoundsIDs.SOUND_SPECIAL_LAUNCH: this.specialLaunch.Play(); break;

                }//switch
            }
        }

        public void stopSound(SoundsIDs id)
        {
            switch (id)
            {
            case SoundsIDs.SOUND_ETYPE_1_SHOT:
                if (this.e1s_shot_I != null)
                {
                    this.e1s_shot_I.Stop();
                    this.e1s_shot_I = null;
                }
                break;

            case SoundsIDs.SOUND_ETYPE_5_SHOT:
                if (this.e5s_shot_I != null)
                {
                    this.e5s_shot_I.Stop();
                    this.e5s_shot_I = null;
                }
                break;

            case SoundsIDs.SOUND_ETYPE_6_SHOT:
                if (this.e6s_shot_I != null)
                {
                    this.e6s_shot_I.Stop();
                    this.e6s_shot_I = null;
                }
                break;

            case SoundsIDs.SOUND_ETYPE_8_SHOT:
                if (this.e8s_shot_I != null)
                {
                    this.e8s_shot_I.Stop();
                    this.e8s_shot_I = null;
                }
                break;

            case SoundsIDs.SOUND_ETYPE_9_SHOT:
                if (this.e9s_shot_I != null)
                {
                    this.e9s_shot_I.Stop();
                    this.e9s_shot_I = null;
                }
                break;

            case SoundsIDs.SOUND_DAMAGE:
                if (this.damageSoundIsPlaying && this.damageSound_I != null)
                {
                    this.damageSoundIsPlaying = false;
                    this.damageSound_I.Stop();
                    this.damageSound_I = null;
                }
                break;

            }//switch
        }

        public void stopAll()
        {
            for (int i = 0; i < this.playingSounds.Count; ++i)
            {
                if (this.playingSounds[i] != null)
                { 
                    this.playingSounds[i].Stop();
                }
            }
            this.playingSounds.Clear();
        }

        public void enableSounds(bool enable)
        {
            this.enabled = enable;
        }
    }
}
