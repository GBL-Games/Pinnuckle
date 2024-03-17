extends Node2D

#Deck Signals
signal dealCards(amount:int, owner:String)
signal giveCards(owner:String, cards:Array) 
signal shuffleCards

#Player Signals
signal playerAction(action:String)
signal playerHpChange(amount:int)
signal playerMpChange(amount:int)
signal playerXpChange(amount:int)