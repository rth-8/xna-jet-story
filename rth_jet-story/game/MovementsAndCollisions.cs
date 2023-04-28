using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace rth_jet_story.game
{
    class MovementsAndCollisions
    {
        public static void collisions_Ship_Walls(Room temp, Ship lod)
        {
            //kolize lode s objkety Wall
            for (int k=0; k<temp.getWallsCount(); k++)
            {
                Wall tmp_wall = temp.getWall(k);

                int col = lod.collision(tmp_wall);

                if (col == (int)GameObject.Collisions.COLLISION_DOWN)
                {
                    float ly = lod.getPosition().Y + lod.getH();
                    float wy = tmp_wall.getPosition().Y;
                    float dy = Math.Abs(ly - wy);
                    lod.setPosition(lod.getPosition().X, lod.getPosition().Y - dy - 1);
                    lod.setVelocity(lod.getVelocity().X, -lod.getVelocity().Y/2);
                }

                if (col == (int)GameObject.Collisions.COLLISION_UP)
                {
                    float ly = lod.getPosition().Y;
                    float wy = tmp_wall.getPosition().Y + tmp_wall.getH();
                    float dy = Math.Abs(ly - wy);
                    lod.setPosition(lod.getPosition().X, lod.getPosition().Y + dy + 1);
                    lod.setVelocity(lod.getVelocity().X, -lod.getVelocity().Y/2);
                }

                if (col == (int)GameObject.Collisions.COLLISION_LEFT)
                {
                    float lx = lod.getPosition().X;
                    float wx = tmp_wall.getPosition().X + tmp_wall.getW();
                    float dx = Math.Abs(lx - wx);
                    lod.setPosition(lod.getPosition().X + dx + 1, lod.getPosition().Y);
                    lod.setVelocity(-lod.getVelocity().X/2, lod.getVelocity().Y);
                }

                if (col == (int)GameObject.Collisions.COLLISION_RIGHT)
                {
                    float lx = lod.getPosition().X + lod.getW();
                    float wx = tmp_wall.getPosition().X;
                    float dx = Math.Abs(lx - wx);
                    lod.setPosition(lod.getPosition().X - dx - 1, lod.getPosition().Y);
                    lod.setVelocity(-lod.getVelocity().X/2, lod.getVelocity().Y);
                }
            }
        }

        public static void collisions_Ship_Enemies(Game canvas, Room temp, Ship lod, List<Boom> booms)
        {
            //qDebug() << "> collisions_Ship_Enemies";

            //kolize lode s objkety Enemy
            for (int k=0; k<temp.getEnemiesCount(); k++)
            {
                Enemy te = temp.getEnemy(k);
                if (te.isDead() == false)
                {
                    if (lod.collision(te) > (int)GameObject.Collisions.NO_COLLISION)
                    {
                        lod.setColliding(true);
                        lod.setDamage(lod.getDamage()-10);
                        te.setDamage(te.getDamage()-10);
                        if (te.getDamage() == 0)
                        {
                            te.setDead();
                            if (te.getType() == 0)
                            {
                                Maze.getMazeInstance().addToScore(1000);
                                Maze.getMazeInstance().decBases();
                            }
                            else
                            {
                                Maze.getMazeInstance().addToScore(100);
                            }
                            switch (te.getType())
                            {
                                case 0: //base
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true, true));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BASE_BOOM);
                                    break;
                                case 18:
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true, false));
                                    booms.Add(new Boom(
                                            te.getPosition().X
                                                + Game.recalculate(25),
                                            te.getPosition().Y, true, false));
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y
                                                + Game.recalculate(25), true, false));
                                    booms.Add(new Boom(
                                            te.getPosition().X
                                                + Game.recalculate(25),
                                            te.getPosition().Y
                                                + Game.recalculate(25), true, false));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_CANON);
                                    break;
                                case 19:
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true, false));
                                    booms.Add(new Boom(
                                            te.getPosition().X
                                                + Game.recalculate(25),
                                            te.getPosition().Y, true, false));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                    break;
                                default:
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true, false));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                    break;
                            }
                        }
                    }

                    //se souputnikem 20
                    if (te.getType() == 20)
                    {
                        //enemy
                        Enemy tee = ((EType20)te).getEnemy();
                        if (tee != null && tee.isDead() == false)
                        {
                            if (lod.collision(tee) > (int)GameObject.Collisions.NO_COLLISION)
                            {
                                lod.setColliding(true);
                                lod.setDamage(lod.getDamage()-10);
                                tee.setDamage(tee.getDamage()-10);
                                if (tee.getDamage() == 0)
                                {
                                    tee.setDead();
                                    Maze.getMazeInstance().addToScore(100);
                                    booms.Add(new Boom(
                                            tee.getPosition().X,
                                            tee.getPosition().Y, true, false));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                }
                            }
                        }
                        //item
                        Item tii = ((EType20)te).getItem();
                        if (tii != null && tii.isShown() == true)
                        {
                            if (lod.collision(tii) > (int)GameObject.Collisions.NO_COLLISION)
                            {
                                switch (tii.getType())
                                {
                                    case 0:
                                        lod.getCannon().setAmmo(1000);
                                        break;
                                    case 1:
                                        lod.getSpecial().setType(2);
                                        lod.getSpecial().setAmmo(50);
                                        break;
                                    case 2:
                                        lod.setFuel(1000);
                                        break;
                                    case 3:
                                        lod.getSpecial().setType(1);
                                        lod.getSpecial().setAmmo(50);
                                        break;
                                    case 4:
                                        lod.getSpecial().setType(0);
                                        lod.getSpecial().setAmmo(50);
                                        break;
                                    case 5:
                                        lod.setDamage(1000);
                                        break;
                                }
                                GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ITEM_GET);
                                canvas.setUpInfoBarRects();
                                tii.hide();
                            }
                        }
                    }
                }
            }

            //qDebug() << "< collisions_Ship_Enemies";
        }

        public static void collisions_Ship_EnemiesFrom10(Room temp, Ship lod, List<Boom> booms)
        {
            //qDebug() << "> collisions_Ship_EnemiesFrom10";

            //kolize lode s objkety Enemy z 10 !!!
            for (int k=0; k<Room.E10; k++)
            {
                Enemy te = temp.getEnemy10(k);
                if (te.getDamage() == -1)
                {
                    continue;
                }
                if (te.isDead() == false)
                {
                    if (lod.collision(te) > (int)GameObject.Collisions.NO_COLLISION)
                    {
                        lod.setColliding(true);
                        lod.setDamage(lod.getDamage()-10);
                        te.setDamage(te.getDamage()-10);
                        if (te.getDamage() == 0)
                        {
                            te.setDamage(0);
                            te.setDead();
                            Maze.getMazeInstance().addToScore(100);
                            booms.Add(new Boom(
                                    te.getPosition().X,
                                    te.getPosition().Y, true, false));
                            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                        }
                    }
                }
            }

            //qDebug() << "< collisions_Ship_EnemiesFrom10";
        }

        public static void collisions_Ship_Items(Game canvas, Room temp, Ship lod)
        {
            //qDebug() << "> collisions_Ship_Items";

            //kolize lode s Items - predmety
            for (int k=0; k<temp.getItemsCount(); k++)
            {
                Item itt = temp.getItem(k);
                if (itt.isShown() == true)
                {
                    if (lod.collision(itt) > 0)
                    {
                        switch (itt.getType())
                        {
                            case 0:
                                lod.getCannon().setAmmo(1000);
                                break;
                            case 1:
                                lod.getSpecial().setType(2);
                                lod.getSpecial().setAmmo(50);
                                break;
                            case 2:
                                lod.setFuel(1000);
                                break;
                            case 3:
                                lod.getSpecial().setType(1);
                                lod.getSpecial().setAmmo(50);
                                break;
                            case 4:
                                lod.getSpecial().setType(0);
                                lod.getSpecial().setAmmo(50);
                                break;
                            case 5:
                                lod.setDamage(1000);
                                break;
                        }
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_ITEM_GET);
                        canvas.setUpInfoBarRects();
                        itt.hide();
                    }
                }
            }

            //qDebug() << "< collisions_Ship_Items";
        }

        public static void collisions_Cannon_Enemies(Room temp, Ship lod, List<Boom> booms)
        {
            //qDebug() << "> collisions_Cannon_Enemies";

            //kolize strely Cannon s Enemy
            for (int k=0; k<temp.getEnemiesCount(); k++)
            {
                Enemy te = temp.getEnemy(k);
                if (te.isDead() == false)
                {
                    if (lod.getCannon().collision(te) > 0)
                    {
                        te.setDamage(te.getDamage()-10);
                        if (te.getDamage() == 0)
                        {
                            te.setDead();
                            switch (te.getType())
                            {
                                case 0: //base
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true, true));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BASE_BOOM);
                                    break;
                                case 18:
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true));
                                    booms.Add(new Boom(
                                            te.getPosition().X
                                                + Game.recalculate(25),
                                            te.getPosition().Y, true));
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y
                                                + Game.recalculate(25), true));
                                    booms.Add(new Boom(
                                            te.getPosition().X
                                                + Game.recalculate(25),
                                            te.getPosition().Y
                                                + Game.recalculate(25), true));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                    break;
                                case 19:
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true));
                                    booms.Add(new Boom(
                                            te.getPosition().X
                                                + Game.recalculate(25),
                                            te.getPosition().Y, true));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                    break;
                                default:
                                    booms.Add(new Boom(
                                            te.getPosition().X,
                                            te.getPosition().Y, true));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                    break;
                            }
                            if (te.getType() == 0)
                            {
                                Maze.getMazeInstance().decBases();
                                Maze.getMazeInstance().addToScore(1000);
                            }
                            else
                            {
                                Maze.getMazeInstance().addToScore(100);
                            }
                        }
                        lod.getCannon().hide();
                    }
                    // kolize Cannon se souputnikem 20
                    if (te.getType() == 20)
                    {
                        Enemy tee = ((EType20)te).getEnemy();
                        if (tee != null && tee.isDead() == false)
                        {
                            if (lod.getCannon().collision(tee) > 0)
                            {
                                tee.setDamage(tee.getDamage()-10);
                                if (tee.getDamage() == 0)
                                {
                                    tee.setDead();
                                    Maze.getMazeInstance().addToScore(100);
                                    booms.Add(new Boom(
                                            tee.getPosition().X,
                                            tee.getPosition().Y, true));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                }
                                lod.getCannon().hide();
                            }
                        }
                    }
                }
            }
            //qDebug() << "< collisions_Cannon_Enemies";
        }

        public static void collisions_Cannon_EnemiesFrom10(
                Room temp, Ship lod, List<Boom> booms)
        {
            //qDebug() << "> collisions_Cannon_EnemiesFrom10";

            //kolize strely Cannon s Enemy z 10 !!!
            for (int k=0; k<Room.E10; k++)
            {
                Enemy te = temp.getEnemy10(k);
                if (te == null || te.getDamage() == -1)
                {
                    continue;
                }
                if (te.isDead() == false)
                {
                    if (lod.getCannon().collision(te) > 0)
                    {
                        te.setDamage(te.getDamage()-10);
                        if (te.getDamage() == 0)
                        {
                            te.setDead();
                            booms.Add(new Boom(
                                    te.getPosition().X,
                                    te.getPosition().Y, true));
                            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                            te.setDamage(0);
                            Maze.getMazeInstance().addToScore(100);
                        }
                        lod.getCannon().hide();
                    }
                }
            }

            //qDebug() << "< collisions_Cannon_EnemiesFrom10";
        }

        public static void movement_Cannon(Room temp, Ship lod, List<Boom> booms, float t)
        {
            //qDebug() << "> movement_Cannon";

            //pohyb strely Cannon
            if (lod.getCannon().isShown() == true)
            {
                //pohyb strely Cannon
                if (lod.getCannon().isShown() == true)
                {
                    lod.getCannon().move(t);
                    lod.getCannon().updateMembers();
                }

                //kolize strely s Wall
                for (int k=0; k<temp.getWallsCount(); k++)
                {
                    if (lod.getCannon().collision(temp.getWall(k)) > 0)
                    {
                        lod.getCannon().hide();
                        break;
                    }
                }

                collisions_Cannon_Enemies(temp, lod, booms);

                collisions_Cannon_EnemiesFrom10(temp, lod, booms);
            }

            //qDebug() << "< movement_Cannon";
        }

        public static void collisions_Special_Walls(Room temp, Ship lod)
        {
            //qDebug() << "> collisions_Special_Walls";

            //kolize strely Special s Wall
            for (int k=0; k<temp.getWallsCount(); k++)
            {
                Wall tw = temp.getWall(k);
                switch (lod.getSpecial().getType())
                {
                    case 0:
                        //qDebug() << "+ collisions_Special_Walls - missile";
                    case 1:
                        //qDebug() << "+ collisions_Special_Walls - missile";
                        if (lod.getSpecial().collision(tw) > 0)
                        {
                            lod.getSpecial().hide();
                            lod.getSpecial().setType(lod.getSpecial().getToChange());
                        }
                        break;
                    case 2: //kulicka
                        /*
                        //qDebug() << "+ collisions_Special_Walls - marble";
                        int col = lod.getSpecial().collision(tw);
                        if (col == (int)GameObject.Collisions.COLLISION_LEFT) {
                        //if (lod.getSpecial().leftCollide(tw) == true) {
                            lod.getSpecial().moveObjectToNewPosition(tw, col);
                            lod.getSpecial().setVelocity(
                                    -(lod.getSpecial().getVelocity().X),
                                    lod.getSpecial().getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_RIGHT) {
                        //if (lod.getSpecial().rightCollide(tw) == true) {
                            lod.getSpecial().moveObjectToNewPosition(tw, col);
                            lod.getSpecial().setVelocity(
                                    -(lod.getSpecial().getVelocity().X),
                                    lod.getSpecial().getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_UP) {
                        //if (lod.getSpecial().upCollide(tw) == true) {
                            lod.getSpecial().moveObjectToNewPosition(tw, col);
                            lod.getSpecial().setVelocity(
                                    lod.getSpecial().getVelocity().X,
                                    -(lod.getSpecial().getVelocity().Y));
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_DOWN) {
                        //if (lod.getSpecial().downCollide(tw) == true) {
                            lod.getSpecial().moveObjectToNewPosition(tw, col);
                            lod.getSpecial().setVelocity(
                                    lod.getSpecial().getVelocity().X,
                                    -(lod.getSpecial().getVelocity().Y));
                        }
                        */
                        break;
                }//switch
            }//for

            //kulicka
            if (lod.getSpecial().getType() == 2)
            {
                if (lod.getSpecial().doCollisionAndBounce(temp.getWalls(), temp.getWallsCount(), 1))
                {
                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_CIRLCE_BOUNCE);
                }
            }

            //qDebug() << "< collisions_Special_Walls";
        }

        public static void collisions_Special_Enemies(Room temp, Ship lod, List<Boom> booms)
        {
            //qDebug() << "> collisions_Special_Enemies";

            //kolize strely Special s Enemy
            for (int k=0; k<temp.getEnemiesCount(); k++)
            {
                Enemy te = temp.getEnemy(k);
                if (te.isDead() == false)
                {
                    if (lod.getSpecial().collision(te) > 0)
                    {
                        te.setDead();
                        switch (te.getType())
                        {
                            case 0: //base
                                booms.Add(new Boom(
                                        te.getPosition().X,
                                        te.getPosition().Y, true, true));
                                GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BASE_BOOM);
                                break;
                            case 18:
                                booms.Add(new Boom(
                                        te.getPosition().X,
                                        te.getPosition().Y, true));
                                booms.Add(new Boom(
                                        te.getPosition().X
                                            + Game.recalculate(25),
                                        te.getPosition().Y, true));
                                booms.Add(new Boom(te.getPosition().X,
                                        te.getPosition().Y
                                            + Game.recalculate(25), true));
                                booms.Add(new Boom(
                                        te.getPosition().X
                                            + Game.recalculate(25),
                                        te.getPosition().Y
                                            + Game.recalculate(25), true));
                                GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                break;
                            case 19:
                                booms.Add(new Boom(
                                        te.getPosition().X,
                                        te.getPosition().Y, true));
                                booms.Add(new Boom(
                                        te.getPosition().X
                                            + Game.recalculate(25),
                                        te.getPosition().Y, true));
                                GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                break;
                            default:
                                booms.Add(new Boom(
                                        te.getPosition().X,
                                        te.getPosition().Y, true));
                                GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                break;
                        }
                        if (te.getType() == 0)
                        {
                            Maze.getMazeInstance().decBases();
                            Maze.getMazeInstance().addToScore(1000);
                        }
                        else
                        {
                            Maze.getMazeInstance().addToScore(100);
                        }
                        switch (lod.getSpecial().getType())
                        {
                            case 0:
                            case 1:
                                lod.getSpecial().hide();
                                break;
                            case 2: //kulicka
                                break;
                        }
                    }
                    // kolize Special se souputnikem 20
                    if (te.getType() == 20)
                    {
                        Enemy tee = ((EType20)te).getEnemy();
                        if (tee != null && tee.isDead() == false)
                        {
                            if (lod.getSpecial().collision(tee) > 0)
                            {

                                tee.setDead();
                                Maze.getMazeInstance().addToScore(100);
                                booms.Add(new Boom(
                                        tee.getPosition().X,
                                        tee.getPosition().Y, true));
                                GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                switch (lod.getSpecial().getType())
                                {
                                    case 0:
                                    case 1:
                                        lod.getSpecial().hide();
                                        break;
                                    case 2: //kulicka
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //qDebug() << "< collisions_Special_Enemies";
        }

        public static void collisions_Special_EnemiesFrom10(Room temp, Ship lod, List<Boom> booms)
        {
            //qDebug() << "> collisions_Special_EnemiesFrom10";

            //kolize strely Special s Enemy z 10 !!!
            for (int k=0; k<Room.E10; k++)
            {
                Enemy te = temp.getEnemy10(k);
                if (te == null || te.getDamage() == -1)
                {
                    continue;
                }
                if (te.isDead() == false)
                {
                    if (lod.getSpecial().collision(te) > 0)
                    {
                        te.setDead();
                        Maze.getMazeInstance().addToScore(100);
                        booms.Add(new Boom(
                                te.getPosition().X,
                                te.getPosition().Y, true));
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                        te.setDamage(0);
                        if (te.getType() == 0)
                        {
                            Maze.getMazeInstance().decBases();
                        }
                        switch (lod.getSpecial().getType())
                        {
                            case 0:
                            case 1:
                                lod.getSpecial().hide();
                                break;
                            case 2: //kulicka
                                break;
                        }
                    }
                }
            }

            //qDebug() << "< collisions_Special_EnemiesFrom10";
        }

        public static void movement_Special(Room temp, Ship lod, List<Boom> booms, float t)
        {
            //qDebug() << "> movement_Special";

            //pohyb strely Special
            if (lod.getSpecial().isShown() == true)
            {
                //pohyb strely Special
                if (lod.getSpecial().isShown() == true)
                {
                    lod.getSpecial().move(t);
                    lod.getSpecial().updateMembers();
                }

                collisions_Special_Walls(temp, lod);

                collisions_Special_Enemies(temp, lod, booms);

                collisions_Special_EnemiesFrom10(temp, lod, booms);
            }
            //qDebug() << "< movement_Special";
        }

        public static void collisions_Enemy20_or_ItemFrom20_Walls(
                Room temp, int k)
        {
            //qDebug() << "> collisions_Enemy20_or_ItemFrom20_Walls";

            //kolize Enemy | Item - 20 - s Wall
            Enemy te = temp.getEnemy(k);
            if (te != null && te.getType() == 20)
            {
                //enemy
                Enemy tee = ((EType20)te).getEnemy();
                if (tee != null && tee.isDead() == false)
                {
                    for (int l=0; l<temp.getWallsCount(); l++)
                    {
                        Wall tw = temp.getWall(l);
                        int col = tee.collision(tw);
                        if (col == (int)GameObject.Collisions.COLLISION_UP)
                        {
                            //qDebug() << "fellow enemy UP collison";
                            te.moveObjectToNewPosition(tee, tw, col);
                            te.setVelocity(
                                    te.getVelocity().X, -te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_DOWN)
                        {
                            //qDebug() << "fellow enemy DOWN collison";
                            te.moveObjectToNewPosition(tee, tw, col);
                            te.setVelocity(
                                    te.getVelocity().X, -te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_LEFT)
                        {
                            //qDebug() << "fellow enemy LEFT collison";
                            te.moveObjectToNewPosition(tee, tw, col);
                            te.setVelocity(
                                    -te.getVelocity().X, te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_RIGHT)
                        {
                            //qDebug() << "fellow enemy RIGHT collison";
                            te.moveObjectToNewPosition(tee, tw, col);
                            te.setVelocity(
                                    -te.getVelocity().X, te.getVelocity().Y);
                        }
                    }
                }
                //item
                Item iee = ((EType20)te).getItem();
                if (iee != null && iee.isShown() == true)
                {
                    for (int l=0; l<temp.getWallsCount(); l++)
                    {
                        Wall tw = temp.getWall(l);
                        int col = iee.collision(tw);
                        if (col == (int)GameObject.Collisions.COLLISION_UP)
                        {
                            te.moveObjectToNewPosition(iee, tw, col);
                            te.setVelocity(
                                    te.getVelocity().X, -te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_DOWN)
                        {
                            te.moveObjectToNewPosition(iee, tw, col);
                            te.setVelocity(
                                    te.getVelocity().X, -te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_LEFT)
                        {
                            te.moveObjectToNewPosition(iee, tw, col);
                            te.setVelocity(
                                    -te.getVelocity().X, te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_RIGHT)
                        {
                            te.moveObjectToNewPosition(iee, tw, col);
                            te.setVelocity(
                                    -te.getVelocity().X, te.getVelocity().Y);
                        }
                    }
                }
            }

            //qDebug() << "< collisions_Enemy20_or_ItemFrom20_Walls";
        }

        public static void collisions_Enemy_Walls(Room temp, int k)
        {
            //qDebug() << "> collisions_Enemy_Walls";

            //kolize Enemy s Wall
            for (int l=0; l<temp.getWallsCount(); l++)
            {
                Enemy te = temp.getEnemy(k);
                Wall tw = temp.getWall(l);
                int col = te.collision(tw);
                if (col == (int)GameObject.Collisions.COLLISION_UP)
                {
                    te.moveObjectToNewPosition(tw, col);
                    te.setVelocity(
                            te.getVelocity().X, -te.getVelocity().Y);
                }
                if (col == (int)GameObject.Collisions.COLLISION_DOWN)
                {
                    te.moveObjectToNewPosition(tw, col);
                    te.setVelocity(
                            te.getVelocity().X, -te.getVelocity().Y);
                }
                if (col == (int)GameObject.Collisions.COLLISION_LEFT)
                {
                    te.moveObjectToNewPosition(tw, col);
                    te.setVelocity(
                            -te.getVelocity().X, te.getVelocity().Y);
                }
                if (col == (int)GameObject.Collisions.COLLISION_RIGHT)
                {
                    te.moveObjectToNewPosition(tw, col);
                    te.setVelocity(
                            -te.getVelocity().X, te.getVelocity().Y);
                }
            }

            //qDebug() << "< collisions_Enemy_Walls";
        }

        public static void collisions_ShotFromEnemy20_Walls(Room temp, int k)
        {
            //qDebug() << "> collisions_ShotFromEnemy20_Walls";

            // strela souputnika nepritele c 20 !!!
            if (temp.getEnemy(k).getType() == 20)
            {
                //qDebug() << "get fellow enemy from 20";
                Enemy e20 = ((EType20)temp.getEnemy(k)).getEnemy();
                // continue only if fellow enemy really exists,
                // because 20 can carry item
                if (e20 != null && e20.getShot() != null) {
                    //qDebug() << "found";
                    if (e20.isDead() == false) {
                        //qDebug() << "alive";
                        for (int l=0; l<temp.getWallsCount(); l++)
                        {
                            Shot e20s = e20.getShot();
                            // try to calculate colisions only if fellow
                            // enemy is really shooting
                            if (e20s != null && e20s.collision(temp.getWall(l)) > 0)
                            {
                                //qDebug() << "shot collision";
                                e20.getShot().hide();
                            }
                        }
                    }
                }
            }

            //qDebug() << "< collisions_ShotFromEnemy20_Walls";
        }

        public static void collisions_ShotFromEnemy20_Ship(Room temp, Ship lod, int k)
        {
            //qDebug() << "> collisions_ShotFromEnemy20_Ship";

            //strela souputnika nepritele c 20 !!!
            if (temp.getEnemy(k).getType() == 20)
            {
                Enemy e20 = ((EType20)temp.getEnemy(k)).getEnemy();
                if (e20 != null && e20.isDead() == false)
                {
                    Shot e20s = e20.getShot();
                    if (e20s != null && e20s.isShown() == true && e20s.collision(lod) > 0)
                    {
                        lod.setDamage(lod.getDamage()-10);
                        e20.getShot().hide();
                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_SHIP_DAMAGE);
                    }
                }
            }

            //qDebug() << "< collisions_ShotFromEnemy20_Ship";
        }

        public static void collisions_ShotFromEnemy_Walls(Room temp, List<Boom> booms, int k)
        {
            //qDebug() << "> collisions_ShotFromEnemy_Walls";

            //kolize strel Enemy s Wall
            for (int l=0; l<temp.getWallsCount(); l++)
            {
                if (temp.getEnemy(k).getShot().collision(temp.getWall(l)) > 0)
                {
                    temp.getEnemy(k).getShot().hide();
                    switch (temp.getEnemy(k).getType())
                    {
                        case 1:
                        case 5:
                        case 6:
                        case 8:
                        case 9:
                            booms.Add(new Boom(
                                    temp.getEnemy(k).getShot().getPosition().X,
                                    temp.getEnemy(k).getShot().getPosition().Y,
                                    true));
                            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                            break;
                    }
                }
            }

            //qDebug() << "< collisions_ShotFromEnemy_Walls";
        }

        public static void collisions_ShotFromEnemy_Ship(Room temp, Ship lod, List<Boom> booms, int k)
        {
            //qDebug() << "> collisions_ShotFromEnemy_Ship";

            Shot shot = temp.getEnemy(k).getShot();
            if (shot != null && shot.isShown())
            {
                //qDebug() << "+ collisions_ShotFromEnemy_Ship: exists and shown";
                //kolize strel Enemy s lodi
                if (shot.collision(lod) > 0)
                {
                    //qDebug() << "+ collisions_ShotFromEnemy_Ship: colliding with ship";
                    lod.setDamage(lod.getDamage()-10);
                    shot.hide();
                    switch (temp.getEnemy(k).getType())
                    {
                        case 1:
                        case 5:
                        case 6:
                        case 8:
                        case 9:
                            booms.Add(new Boom(
                                    temp.getEnemy(k).getShot().getPosition().X,
                                    temp.getEnemy(k).getShot().getPosition().Y,
                                    true));
                            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                            break;
                        default:
                            GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_SHIP_DAMAGE);
                            break;
                    }
                    //smrt po kolizi se strelou enemy 9
                    if (temp.getEnemy(k).getType() == 9)
                    {
                        lod.setDamage(0);
                    }
                }
            }//isShown

            //qDebug() << "< collisions_ShotFromEnemy_Ship";
        }

        public static void collisions_ShotFromEnemy_Enemies(Room temp, int k)
        {
            //qDebug() << "> collisions_ShotFromEnemy_Enemies";

            //kolize strel Enemy s jinyn objektem Enemy
            for (int l=0; l<temp.getEnemiesCount(); l++)
            {
                if (l == k || temp.getEnemy(l).isDead() == true)
                {
                    continue;
                }
                if (temp.getEnemy(k).getShot().collision(temp.getEnemy(l)) > 0)
                {
                    temp.getEnemy(k).getShot().hide();
                }
            }

            //qDebug() << "< collisions_ShotFromEnemy_Enemies";
        }

        public static void movement_ShotsFromEnemies(Room temp, Ship lod, List<Boom> booms, int k, float t)
        {
            //qDebug() << "> movement_ShotsFromEnemies";

            Shot shot = temp.getEnemy(k).getShot();
            //pohyb strely nepritele - podle typu
            if (shot != null && shot.isShown())
            {
                //qDebug() << "+ movement_ShotsFromEnemies - enemy #" << k << " can shoot";
                //qDebug() << "+ movement_ShotsFromEnemies - enemy #" << k << " is shooting";

                shot.move(t);
                shot.updateMembers();

                collisions_ShotFromEnemy_Walls(temp, booms, k);

                collisions_ShotFromEnemy_Ship(temp, lod, booms, k);

                //zakomentovano, protoze v originale strely
                //prochazeji neprateli
                //this.collisions_ShotFromEnemy_Enemies(temp, k);
            }

            //qDebug() << "< movement_ShotsFromEnemies";
        }

        public static void movement_Enemies(Room temp, Ship lod, List<Boom> booms, float t)
        {
            //qDebug() << "> movement_Enemies";

            //pohyb nepratel - Enemy
            for (int k=0; k<temp.getEnemiesCount(); k++)
            {
                if (temp.getEnemy(k).isDead() == false)
                {
                    //pohyb nepritele
                    //qDebug() << "moving enemy #" << k << " - type " << temp.getEnemy(k).getType();
                    temp.getEnemy(k).move(lod, t);
                    temp.getEnemy(k).updateMembers();

                    collisions_Enemy20_or_ItemFrom20_Walls(temp, k);

                    collisions_Enemy_Walls(temp, k);
                }

                collisions_ShotFromEnemy20_Walls(temp, k);

                collisions_ShotFromEnemy20_Ship(temp, lod, k);

                movement_ShotsFromEnemies(temp, lod, booms, k, t);
            }

            //qDebug() << "< movement_Enemies";
        }

        public static void movement_EnemiesFrom10(Room temp, Ship lod, List<Boom> booms, float t)
        {
            //qDebug() << "> movement_EnemiesFrom10";

            //pohyb nepratel - enemy z 10 - z pole enemies10 !!!
            for (int k=0; k<Room.E10; k++)
            {
                Enemy te = temp.getEnemy10(k);
                if (te.isDead() == false)
                {
                    //pohyb nepritele z 10
                    te.move(lod, t);
                    te.updateMembers();

                    //kolize enemy z 10 s Wall
                    for (int l=0; l<temp.getWallsCount(); l++)
                    {
                        Wall tw = temp.getWall(l);
                        int col = te.collision(tw);
                        if (col == (int)GameObject.Collisions.COLLISION_UP)
                        {
                            te.moveObjectToNewPosition(tw, col);
                            te.setVelocity(
                                    te.getVelocity().X, -te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_DOWN)
                        {
                            te.moveObjectToNewPosition(tw, col);
                            te.setVelocity(
                                    te.getVelocity().X, -te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_LEFT)
                        {
                            te.moveObjectToNewPosition(tw, col);
                            te.setVelocity(
                                    -te.getVelocity().X, te.getVelocity().Y);
                        }
                        if (col == (int)GameObject.Collisions.COLLISION_RIGHT)
                        {
                            te.moveObjectToNewPosition(tw, col);
                            te.setVelocity(
                                    -te.getVelocity().X, te.getVelocity().Y);
                        }
                    }
                }

                //pohyb strely nepritele z 10 - podle typu
                if (te.getShot() != null)
                {
                    if (te.getShot().isShown() == true)
                    {
                        //pohyb strely nepritele z 10
                        te.getShot().move(t);
                        te.getShot().updateMembers();

                        //kolize strel Enemy z 10 s Wall
                        for (int l=0; l<temp.getWallsCount(); l++)
                        {
                            if (te.getShot().collision(temp.getWall(l)) > 
                                (int)GameObject.Collisions.NO_COLLISION)
                            {
                                te.getShot().hide();
                                switch (te.getType())
                                {
                                    case 1:
                                    case 5:
                                    case 6:
                                    case 8:
                                    case 9:
                                        booms.Add(new Boom(
                                            te.getShot().getPosition().X,
                                            te.getShot().getPosition().Y,
                                            true));
                                        GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                        break;
                                }
                            }
                        }

                        //kolize strel Enemy z 10 s lodi
                        if (te.getShot().collision(lod) > (int)GameObject.Collisions.NO_COLLISION)
                        {
                            lod.setDamage(lod.getDamage()-10);
                            te.getShot().hide();
                            switch (te.getType())
                            {
                                case 1:
                                case 5:
                                case 6:
                                case 8:
                                case 9:
                                    booms.Add(new Boom(
                                        te.getShot().getPosition().X,
                                        te.getShot().getPosition().Y,
                                        true));
                                    GameSounds.getInstance().playSound(GameSounds.SoundsIDs.SOUND_BOOM);
                                    break;
                            }
                        }
                    }
                }
            }

            //qDebug() << "< movement_EnemiesFrom10";
        }

        public static void movement_Items(Room temp)
        {
            for (int i=0; i<temp.getItemsCount(); ++i)
            {
                Item item = temp.getItem(i);
                if (item != null)
                {
                    item.move(0);
                }
            }
        }

        public static void movement_Booms(List<Boom> booms, ref Microsoft.Xna.Framework.Color c, float t)
        {
            for (int pos = 0; pos < booms.Count; pos++)
            {
                Boom boom = booms[pos];
                if (!boom.isFinished())
                {
                    boom.move(t);
                }
            }

            bool b = false;
            for (int pos = 0; pos < booms.Count; pos++)
            {
                //Console.WriteLine("boom #" + pos);
                Boom boom = booms[pos];
                if (!boom.isFinished())
                {
                    if (boom.isBase())
                    {
                        //Console.WriteLine("base boom");
                        switch (boom.getActB())
                        {
                        case 0:
                        case 1:
                            c = Microsoft.Xna.Framework.Color.White;
                            break;
                        case 2:
                        case 3:
                            c = Microsoft.Xna.Framework.Color.Yellow;
                            break;
                        case 4:
                        case 5:
                            c = Microsoft.Xna.Framework.Color.Black;
                            break;
                        }
                    }
                    else
                    {
                        c = Microsoft.Xna.Framework.Color.Black;
                    }
                    b = true;
                }
                else
                {
                    //boom finished
                    booms.RemoveAt(pos);
                    c = Microsoft.Xna.Framework.Color.Black;
                }
            }

            if (b == false)
            {
                booms.Clear();
                c = Microsoft.Xna.Framework.Color.Black;
            }
        }

        public static void collisions_Fragments_Walls(Room temp, List<Boom> booms, float t)
        {
            for (int i=0; i<booms.Count; ++i)
            {
                Boom boom = booms[i];
                if (!boom.isFinished())
                {
                    for (int j=0; j<Boom.FRAGS; ++j)
                    {
                        Fragment frag = boom.getFragment(j);
                        if (frag.isShown())
                        {
                            for (int k=0; k<temp.getWallsCount(); ++k)
                            {
                                if (frag.collision(temp.getWall(k)) > 0)
                                {
                                    frag.hide();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
