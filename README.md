# WantedBountyHunt
[Rust Plugin] PVP Minigame

**__Wanted Bounty Hunt__**
- I went by this bus stop and it seems it has this place for a poster. The bus stop has the spot for a poster on both sides. So we would place 4 posters in each bus stop on the map (2 on the inside, and 2 on the outside of the bus stop). https://gyazo.com/b6c31d3d68d7866a879b8ac45fed6c7c
- It would be good if we could find other spots where we can place these signs through code in the Outpost and Bandit Camp, as well as in monuments. Maybe we could paste them in all of these things found along roads https://gyazo.com/e36e09546c56b9581c21a66a2d01eab8
	Only bus stops would be enough tho.
- We would use this spot (if possible) to spawn a Wanted sign assigned to the wanted player. You can see how it can be easily assigned to a player in your friend's list, so hopefully this can also be done through code and assign the player you want..
  	1) Empty sign: https://gyazo.com/6fed1a2fce1c4bdf1bff130c9a20cd6a
  	2) Player selection: https://gyazo.com/f681355bcc64385e37057d5300ce8d74
- The plugin is like a bounty-hunter game. The player with most PVP kills is set as the target automatically, and the default bounty is set (100 scrap for example).
- The bounty is increased everytime wanted player kills another player (increasing in 25 scrap for each new player killed, for example).
- If another player gets more player kills than the wanted player before the wanted player is killed, he becomes the new wanted player, and inherits the bounty amount the other player had, incremented once. So if player X was the wanted with a 275 scrap bounty, and player Y passes him in PVP kills, then player Y would become the new wanted player with a 300 scrap bounty.
- Whoever kills the wanted player, gets the reward, and the PVP kills count gets resetted, and wanted signs get empty, or just removed, until the first player gets a kill again and we start all over.
- As an extra option, i would be nice if the wanted player appeared marked in the compass, but NOT in the map, only in the compass, and only under some conditions:
  	1) the player is online
  	2) the player is not in the range of his TC
	So basically people could not track him when he's offline, and his base would be more protected. You could even make a protection system, so wanted player doesnt appear marked in the compass if in the range of his TC, and the protection lasts 30 seconds from the time he leaves his TC range.
- Anyone that kills the wanted player would get the reward, but only the players that interact with the POSTER get the wanted player marked in the compass, so people don't have that annoying mark on the compass if not interested at all.
