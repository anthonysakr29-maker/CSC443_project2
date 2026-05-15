# CSC443_project2

# Lab Breach

Lab Breach is a first-person wave survival shooter made in Unity for CSC443 Project 2.

The player must survive waves of rogue robots inside an abandoned lab, collect weapons and pickups, earn money, and use the shop between waves to buy upgrades and activate a defensive turret.

## Controls

- WASD — Move

- Mouse — Look

- Left Click — Shoot

- Q / Mouse Wheel — Switch Weapon

- Shift — Sprint

- Space — Jump

## Gameplay Objective

Survive as many waves as possible.

Enemies spawn from different chambers around the arena. Killing enemies gives score and money. Money can be used in the shop between waves.

## Features Implemented

- Main menu with styled presentation

- How To Play panel

- Game Over screen with final score, money, and wave reached

- Player health system

- Player health bar and damage flash

- Wave-based enemy spawning

- Scaling difficulty across waves

- Three enemy types:

- Normal Robot

- Fast Robot

- Tank Robot

- Enemy charge attack behavior with wind-up cue

- Enemy health bars

- Score and money system

- Between-wave shop

- Buyable friendly turret

- Turret upgrades:

- Fire rate upgrade

- Damage upgrade

- Health and ammo shop purchases

- Respawning health pickups

- Respawning ammo pickups

- Weapon pickups and weapon switching

- Enemy hit feedback

- Enemy death effects

- Weapon, enemy, turret, menu, and game audio

- Abandoned lab arena environment

- NavMesh enemy navigation

## Enemy Types

### Normal Robot

Balanced enemy with standard health, damage, and movement.

### Fast Robot

Smaller and faster robot with lower health. Designed to pressure the player.

### Tank Robot

Larger and slower robot with higher health and damage. Appears in later waves.

## Shop System

The shop opens between waves and pauses gameplay.

The player can spend earned money on:

- Turret activation

- Turret fire rate upgrade

- Turret damage upgrade

- Full health restore

- Ammo refill

## Pickups

- Weapon pickups unlock weapons.

- Ammo pickups refill the active weapon and respawn after a delay.

- Health pickups heal the player and respawn after a delay.

## Environment

The game takes place in an abandoned industrial lab arena.

The level includes:

- Central turret platform

- Spawn doors

- Cover objects

- Modular assets

- Wall and Ceiling lighting

- Baked navigation setup

## Assets Used

- Unity Starter Assets First Person Controller

- Lab environment asset pack

- Robot model from the starter project / imported assets

- Weapon models from the starter project

- Audio and VFX assets used for combat feedback

## Known Issues

- Lighting may vary slightly depending on quality settings.

- Quit button only closes the game in a built executable, not inside the Unity Editor.

## What I'm Proud of

- Animation on Main Menu

- Not being as overly reliant on AI during the development of the game

- Not the time it took me to make it

## Project Notes

This project was developed using GitHub for version control.

The game focuses on wave survival, enemy variety, player feedback, economy/shop mechanics, and environment polish.
