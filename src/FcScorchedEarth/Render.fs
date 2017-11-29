﻿module Render

open Domain
open TriangleSprite
open Terrain
open Geometry
open OpenTK.Graphics.OpenGL
open TankSprite

let private renderColoredPoint (point : ColoredPoint2d) = 
    let c = point.Color
    let p = point.Point
    GL.Color4(c.R, c.G, c.B, 1.0); 
    GL.Vertex3(p.X,p.Y, 2.)

let private renderPointWithColor (color : Color3) (alpha: float) (point : Point2d) = 
    GL.Color4(color.R, color.G, color.B, alpha); 
    GL.Vertex3(point.X,point.Y, 2.)

let private renderPoint (point : Point2d) = 
    GL.Color4(1.0, 1.0, 1.0, 1.0); 
    GL.Vertex3(point.X,point.Y, 2.)

let private renderTriangleSprite (sprite : TriangleSprite) = 
    PrimitiveType.Triangles |> GL.Begin
        
    let body= 
        sprite.BodyWrtOrigin 
        |> rotateColoredTriangleWrtOrigin sprite.Heading 
        |> translateColoredTriangleByPoint sprite.Position
    
    for point in body.AsSeq do
        renderColoredPoint point

    GL.End()

let private renderTerrain (terrain: Terrain) = 
    PrimitiveType.Lines |> GL.Begin

    renderColoredPoint <| fst terrain.Body
    renderColoredPoint <| snd terrain.Body

    GL.End()

let private renderTank (sprite : TankSprite) = 
    PrimitiveType.Polygon |> GL.Begin
    let tankBody = 
        sprite.Body 
        |> List.map (addPointToColoredPoint sprite.Position)

    for point in tankBody do 
        renderColoredPoint point
    GL.End()

let renderState (state: GameState) = 
    renderTerrain state.Terrain
    renderTriangleSprite state.TriangleSprite
    renderTank state.PlayerTank
    renderTank state.TargetTank
