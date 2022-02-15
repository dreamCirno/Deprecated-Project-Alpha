--[[
-- Author:passion
-- Date:2019-12-09 17:55:38
-- Desc: 常用C#类别名
--]]


GameObject = CS.UnityEngine.GameObject
Transform = CS.UnityEngine.Transform
Camera = CS.UnityEngine.Camera

ResourceManager = CS.GameCore.Resource
AssetHelper = CS.GameCore.Resource.Asset

--- Resource Define ---
LoadAssetAsync = xlua.get_generic_method(CS.CirnoFramework.Runtime.Resource.Base.IAssetsHelper, 'LoadAssetAsync')

LoadSpriteAsync = LoadAssetAsync(CS.UnityEngine.Sprite)
LoadSpriteAtlasAsync = LoadAssetAsync(CS.UnityEngine.U2D.SpriteAtlas)