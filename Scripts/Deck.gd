extends Node2D

@export var cardData: JSON

var localDeck: Array

# Called when the node enters the scene tree for the first time.
func _ready():
	SignalBus.connect("shuffleCards", shuffleCards)
	SignalBus.connect("dealCards", dealCards)
	pass

# Shuffles the deck for use in the local copy of the deck
func shuffleCards() -> void:
	var shuffledCards = []

	for card in cardData.get_data():
		shuffledCards.append(card)
		pass

	for i in range(cardData.get_data().size()):
		var j = floori(randf() * (i+1))
		var tempCard = shuffledCards[i]
		shuffledCards[i] = shuffledCards[j];
		shuffledCards[j] = tempCard
		pass	

	localDeck = shuffledCards
	pass

func dealCards(amount:int, cardsOwner:String) -> void:
	var cards = []
	for i in range(amount):
		cards.append(localDeck[i])
		pass	

	localDeck = localDeck.slice(amount - 1, localDeck.size()) 
	SignalBus.emit_signal("giveCardsw", cardsOwner, cards)
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
# func _process(delta):
# 	pass
