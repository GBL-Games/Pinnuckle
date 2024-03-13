extends Node2D

@export var cardData: JSON

var localDeck: Array
var player1Hand: JSON
var Player2Hand: JSON

# Called when the node enters the scene tree for the first time.
func _ready():
	localDeck = shuffleCards(cardData)
	pass

# Shuffles the deck for use in the local copy of the deck
func shuffleCards(cards: JSON) -> Array:
	var shuffledCards := []

	for card in cards.get_data():
		shuffledCards.append(card)
		pass

	for i in range(cards.get_data().size()):
		var j = floori(randf() * (i+1))
		var tempCard = shuffledCards[i]
		shuffledCards[i] = shuffledCards[j];
		shuffledCards[j] = tempCard
		pass	
	return shuffledCards;

func dealCards():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
# func _process(delta):
# 	pass
