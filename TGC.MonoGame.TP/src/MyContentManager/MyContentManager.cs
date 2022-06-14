using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace TGC.Monogame.TP.Src.MyContentManagers
{
    public class MyContentManager
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public static MyContentDictionary<Effect> Effects;
        public static MyContentDictionary<Model> Models;
        public static MyContentDictionary<Texture> Textures;
        public static MyContentDictionary<Song> Songs;
        public static MyContentDictionary<SpriteFont> SpriteFonts;
        public static MyContentDictionary<SoundEffect> SoundEffects;
        protected static ContentManager Content;

        public static void SetContentManager(ContentManager content){
            Content = content;
            Effects = new MyContentDictionary<Effect>(Content, ContentFolderEffects);
            Models = new MyContentDictionary<Model>(Content, ContentFolder3D);
            Textures = new MyContentDictionary<Texture>(Content, ContentFolderTextures);
            Songs = new MyContentDictionary<Song>(Content, ContentFolderMusic);
            SpriteFonts = new MyContentDictionary<SpriteFont>(Content, ContentFolderSpriteFonts);
            SoundEffects = new MyContentDictionary<SoundEffect>(Content, ContentFolderSounds);
        }  
    }
}