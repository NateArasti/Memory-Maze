using System.Linq;
using UnityEngine;

public class StoriesStorage : MonoBehaviour
{
	public static int StoryCount => Stories.Length;
	public static bool AllStoriesCollected => StoryCount == Stories.Count(story => story.IsCollected);

	public static int GetNotCollectedStory
	{
		get
		{
			var notCollectedStories = Stories.Where(story => !story.IsCollected).ToArray();
			return notCollectedStories[Random.Range(0, notCollectedStories.Length)].Index - 1;
		}
	} // ReSharper disable StringLiteralTypo
	private static readonly StoryData[] Stories =
	{
		new StoryData(
			"Согласно афинской мифологии лабиринт в Кноссе был построен Дедалом для царя Миноса. Он был так хитро построен, что сам его создатель едва смог сбежать оттуда. В лабиринте заключили чудовище-людоеда Минотавра, на съедение которому афиняне должны были отправлять семь юношей и семь девушек ежегодно. Предназначение лабиринта состояло в том, чтобы удерживать чудовище и не дать ему выбраться наружу.",
			"According to Athenian mythology, the labyrinth at Knossos was built by Daedalus for King Minos.It was so cleverly built that its creator himself was barely able to escape from there.In the labyrinth, the monster-man-eating Minotaur was imprisoned, to which the Athenians were to send seven young men and seven girls annually. The purpose of the labyrinth was to keep the monster from getting out.",
			1),
		new StoryData(
			"Позднее на остров в числе юношей для Минотавра прибыл герой Тесей. Его полюбила Ариадна, дочь Миноса, и, чтобы помочь выбраться из Лабиринта, дала ему с собой клубок ниток. Тесей привязал конец нити к двери у входа в лабиринт и, убив Минотавра, благополучно выбрался из запутанных ходов.",
			"Later, the hero Theseus arrived on the island among the youths for the Minotaur. Ariadne, daughter of Minos, fell in love with him, and to help him get out of the Labyrinth, she gave him a ball of thread. Theseus tied the end of the thread to the door at the entrance to the labyrinth and, having killed the Minotaur, safely got out of the tangled passages.",
			2),
		new StoryData(
			"Вероятно, прототипом для легендарного лабиринта стал Кносский дворец, который считался таким же вымыслом, что и лабиринт, вплоть до самого конца XIX века, когда он был обнаружен сначала Миносом Калокериносом, а затем Артуром Эвансом. Сложный комплекс жилых и хозяйственных помещений Кносского дворца напоминает лабиринт, пещерный, подземно-наземный, состоящий из нескольких этажей.",
			"Probably the prototype for the legendary labyrinth was the Palace of Knossos, which was considered the same fiction as the labyrinth until the very end of the 19th century, when it was first discovered by Minos Kalokerinos and then by Arthur Evans. The complex complex of residential and utility premises of the Knossos Palace resembles a labyrinth, cave, underground-ground, consisting of several floors.",
			3),
		new StoryData(
			"В городе Помпет, находилось по крайней мере два декоративных лабиринта. Один из них, Дом с лабиринтом, известен удивительным мозаичным полом, на котором изображена борьба Тесея с Минотавром. Писатель Марсель Брион утверждает, что это «аллегорическое изображение жизни человека и трудностей, которые должна преодолеть душа в этом мире и в мире ином перед тем, как достичь благословенного состояния бессмертия».",
			"In the city of Pompet, there were at least two decorative labyrinths. One of them, the House with a Labyrinth, is famous for its amazing mosaic floor, which depicts Theseus' struggle with the Minotaur. The writer Marcel Brion claims that this is «an allegorical depiction of human life and the difficulties that the soul must overcome in this world and in the next world before reaching the blessed state of immortality».",
			4),
		new StoryData(
			"Критский лабиринт был известен в мифологии тем, что считался жилищем Минотавра - мифического человека с головой быка. Согласно мифу, Пасифая, жена Миноса, царя Крита, родила это существо; его назвали Минотавром, что означает \"бык Миноса\".",
			"The Cretan labyrinth was famous in mythology for being considered the home of the Minotaur, a mythical man with a bull's head. According to the myth, Pasiphae, the wife of Minos, king of Crete, gave birth to this creature; he was named the Minotaur, which means \"the bull of Minos\".",
			5),
		new StoryData(
			"Лабиринт Минотавра спроектировал и построил знаменитый ученый, скульптор, зодчий и механик того времени – Дедал, которого все помнят как создателя крыльев. Упоминается, что Дедал строил Кносский Лабиринт по образцу египетского, описанного Геродотом. Египетский лабиринт насчитывал 3000 подземных и наземных комнат, однако считается, что Дедал воспроизвёл лишь одну сотую его часть.",
			"The Labyrinth of the Minotaur was designed and built by the famous scientist, sculptor, architect and mechanic of that time - Daedalus, whom everyone remembers as the creator of wings. It is mentioned that Daedalus built the Knossos Labyrinth on the model of the Egyptian Labyrinth described by Herodotus. The Egyptian labyrinth consisted of 3000 underground and above-ground rooms, but it is believed that Daedalus reproduced only one hundredth of it.",
			6),
		new StoryData(
			"Кносский дворец-лабиринт имел минимум 5-6 надземных уровней-этажей, соединенных проходами и лестницами, и целый ряд подземных склепов. Попав сюда, человек даже не подозревал, что вошел в лабиринт, который начинал вытягивать жизненные силы из жертвы.",
			"The Knossos labyrinth palace had at least 5-6 above-ground levels-floors, connected by passages and stairs, and a number of underground crypts. Having got here, the person did not even suspect that he had entered the labyrinth, which began to draw out the vitality from the victim.",
			7),
		new StoryData(
			"Лабиринт Реймсского собора — несохранившийся лабиринт, выложенный из плиток на полу Реймсского собора. Содержал условные портреты и имена первых зодчих, возводивших собор в XIII веке. Созданный около 1290 года, он был разрушен в 1779 году и дошёл до нас только в зарисовке реймсского художника Жака Селье. В XX веке его стилизованное изображение стало логотипом исторических памятников Франции.",
			"The Reims Cathedral Labyrinth is an intact labyrinth laid out of tiles on the floor of the Reims Cathedral. It contained conventional portraits and names of the first architects who erected the cathedral in the 13th century. Created around 1290, it was destroyed in 1779 and has come down to us only in a sketch by the Reims artist Jacques Cellier. In the 20th century, its stylized image became the logo of the historical monuments of France.",
			8),
		new StoryData(
			"Лабиринты подобные Лабиринту Реймсского собора существовали и в других церквях, для верующих он являлся неким аналогом Крестного пути, однако позже данный лабиринт стал забавой для детей, и в 1779 году было принято решение уничтожить его.",
			"Labyrinths similar to the Labyrinth of Reims Cathedral existed in other churches, for believers it was a kind of analogue of the Way of the Cross, but later this labyrinth became fun for children, and in 1779 it was decided to destroy it.",
			9),
		new StoryData(
			"Что было самой выдающейся постройкой египтян? По словам некоторых писателей, это были не пирамиды, как полагает большинство, а огромный лабиринт. Он был построен рядом с озером Мойрис, известным сегодня как озеро Биркет-Карун, расположенным к западу от реки Нил - в 80 километрах к югу от современного города Каира.",
			"What was the most outstanding building of the Egyptians? According to some writers, these were not pyramids, as most believe, but a huge labyrinth. It was built next to Lake Moiris, today known as Lake Birket Karun, located west of the Nile River - 80 kilometers south of the modern city of Cairo.",
			10),
		new StoryData(
			"В V веке до н. э. греческий историк Геродот написал: \"Я видел этот лабиринт: он выше всякого описания. Ведь если бы собрать все стены и великие сооружения, воздвигнутые эллинами, то в общем оказалось бы, что на них затрачено меньше труда и денежных средств, чем на один этот лабиринт\". Он добавил: \"Лабиринт размерами превосходит... пирамиды\". Лабиринт был построен на заре египетской истории. В нем было 3000 помещений, поровну поделенных между подземным и надземным этажами. Лабиринт занимал пространство общей площадью 70 тысяч квадратных метров.",
			"In the 5th century BC Greek historian Herodotus wrote: \"I saw this labyrinth: it is beyond any description. After all, if you collect all the walls and great structures erected by the Hellenes, then in general it would turn out that they spent less labor and money than this one labyrinth \". He added: \"The maze is bigger than ... the pyramids.\" The labyrinth was built at the dawn of Egyptian history. It had 3,000 rooms, divided equally between the underground and aboveground floors. The labyrinth occupied a space with a total area of ​​70 thousand square meters.",
			11),
		new StoryData(
			"В Египетских пирамидах прятался бог-царь Египта, по египетским мифам считалось что лабиринт с его запутанной системой переходов защищал бог-царя в этой и следующей жизни от врагов и даже от самой смерти",
			"The god-king of Egypt hid in the Egyptian pyramids, according to Egyptian myths it was believed that the labyrinth with its intricate system of transitions protected the god-king in this and next life from enemies and even from death itself",
			12),
		new StoryData(
			"Дети в Римской империи играли в лабиринтах, выложенных на полях или на мостовых. Сегодня повсюду в Европе можно встретить много фрагментов мозаичных полов, выполненных в виде лабиринтов, которые были найдены во время раскопок римских вилл и гражданских построек. Мифологические представления вскоре распространились еще дальше.",
			"Children in the Roman Empire played in labyrinths laid out in fields or on pavements. Today, throughout Europe, you can find many fragments of mosaic floors, made in the form of labyrinths, which were found during the excavation of Roman villas and civil buildings. Mythological ideas soon spread even further.",
			13),
		new StoryData(
			"В Скандинавии на побережье Балтийского моря находится более 600 каменных лабиринтов. Есть мнение, что многие из них построены местными рыбаками, которые верили, что, проходя через них, обеспечивают себе хороший улов и счастливое возвращение",
			"In Scandinavia, on the Baltic Sea coast, there are more than 600 stone labyrinths. It is believed that many of them were built by local fishermen who believed that by passing through them they ensure a good catch and a happy return.",
			14),
		new StoryData(
			"Китайцы верили, что злые духи могут летать только по прямой, поэтому они строили входы в виде лабиринтов, чтобы уберечь свои дома и города от злых духов.",
			"The Chinese believed that evil spirits could only fly in a straight line, so they built labyrinthine entrances to protect their homes and cities from evil spirits.",
			15),
		new StoryData(
			"Возникшее на заре истории (первые изображения лабиринта обнаружены в верхнем палеолите 38. 000 лет до н. э. ) простое изображение лабиринта с петляющими дорожками было знакомо многим культурам. Неизвестно, какой народ придумал его первым. Где бы ни жил человек, в Перу или Швеции, в Англии или России, он представляет один образ: путаные дорожки, ложные ходы и тупики, долгожданный выход, который трудно найти.",
			"Appearing at the dawn of history (the first images of the labyrinth were found in the Upper Paleolithic 38,000 BC), a simple image of a labyrinth with winding paths was familiar to many cultures. It is not known which people came up with it first. Wherever a person lives, in Peru or Sweden, in England or Russia, he represents one image: confused paths, false moves and dead ends, a long-awaited exit that is difficult to find.",
			16),
		new StoryData(
			"Начиная с XVI века, по Европе прокатилась волна увлечения садовыми лабиринтами. Стенами лабиринтов служили высокие живые изгороди. Лабиринты с живой изгородью из деревьев и кустарников особенно пылко любили и любят англичане.Один из самых известных английских лабиринтов подобного рода - Хэмптон Корт - был построен, вернее, посажен в 1691 году.",
			"Since the 16th century, a wave of enthusiasm for garden labyrinths swept across Europe. The walls of the labyrinths were high hedges. Labyrinths with a hedge of trees and shrubs were especially passionately loved and loved by the British. One of the most famous English labyrinths of this kind - Hampton Court - was built, or rather, planted in 1691.",
			17),
		new StoryData(
			"Многие древние легенды и сказания народов мира говорят о лабиринтах как о «входах» в подземное (потустороннее) царство, открывающихся тем, кто знал соответствующие заклятия, или оказывался поблизости в тот момент, когда этот вход открывался",
			"Many ancient legends and tales of the peoples of the world speak of labyrinths as «entrances» to the underground (otherworldly) kingdom, opening to those who knew the corresponding spells, or were nearby at the moment when this entrance was opened",
			18),
		new StoryData(
			"Лабиринт — магический символ некоего посвящения. Он якобы символизирует проникновение в иной мир, более совершенный по сравнению с нашим, а путь к центру — путь постижения, путь к познанию. Достигнув этого центра, \"посвященный\" возвращался обратно в свой мир.",
			"The labyrinth is a magical symbol of some kind of initiation. It supposedly symbolizes penetration into another world, more perfect than ours, and the path to the center is the path of comprehension, the path to knowledge. Having reached this center, the \"initiate\" returned back to his own world.",
			19),
		new StoryData(
			"Древние люди изображали окружающий их мир в виде круга или концентрических окружностей, а мир мертвых - в виде спирали или лабиринта. Например, аборигены Австралии изображали на могилах лабиринты как символ переселения умершего в иной непостижимый мир.",
			"Ancient people depicted the world around them as a circle or concentric circles, and the world of the dead as a spiral or labyrinth. For example, the aborigines of Australia depicted labyrinths on the graves as a symbol of the resettlement of the deceased to another incomprehensible world.",
			20),
		new StoryData(
			"В Пакистане и Исландии, чтобы отпугивать воришек, символы лабиринта вырезают на самом высоком дереве в саду. В Шри-Ланке рисунок лабиринта вплетают в ткань для одеял и в основу ивовых корзин. В Скандинавии и Индии если хотят, чтобы исполнилось заветное желание, выкладывают лабиринт из камней в пустынных местах или на побережье. Правда, считается, что в обмен на исполненную мечту лабиринт может забрать у человека семь лет жизни.",
			"In Pakistan and Iceland, to scare off thieves, the symbols of the labyrinth are carved into the tallest tree in the garden. In Sri Lanka, the pattern of the labyrinth is woven into the fabric for blankets and into the base of willow baskets. In Scandinavia and India, if they want to fulfill their cherished desire, they lay out a labyrinth of stones in desert places or on the coast. True, it is believed that in exchange for a fulfilled dream, a labyrinth can take seven years of a person's life.",
			21),
		new StoryData(
			"Лабиринты в соборах Шартра, Реймса, Арраса и Санса стали своеобразной имитацией паломнического пути в Палестину, их порой назывались «Путь в Иерусалим». В те времена для большинства верующих поход на Святую землю был невозможен, и они совершали его в символической форме — проходили весь церковный лабиринт на коленях, читая молитвы.",
			"The labyrinths in the cathedrals of Chartres, Reims, Arras and Sans became a kind of imitation of the pilgrimage route to Palestine, they were sometimes called «The Way to Jerusalem». In those days, for most believers, a trip to the Holy Land was impossible, and they made it in a symbolic form - they went through the entire church labyrinth on their knees, reading prayers.",
			22),
		new StoryData(
			"Лабиринт в христианской философии и архитектуре становится метафорой материального мира, проходя через который человек должен сразиться с Минотавром — Сатаной. В лабиринте соблазнов и грехов человек, подобно Тесею, может уповать только на собственную стойкость и спасительную нить Ариадны — Веру.",
			"The labyrinth in Christian philosophy and architecture becomes a metaphor for the material world, passing through which a person must fight the Minotaur - Satan. In the labyrinth of temptations and sins, a person, like Theseus, can only rely on his own endurance and the saving thread of Ariadne - Faith.",
			23),
		new StoryData(
			"Чтобы избежать коварства лабиринта, нужно оставить какую-нибудь вещь в дар, например, бросить монетку.",
			"To avoid the insidiousness of the labyrinth, you need to leave something as a gift, for example, toss a coin.",
			24),
		new StoryData(
			"Главной неразгаданной загадкой древнего символа остается его происхождение. Десятки гипотез, высказанных на этот счет, так и не смогли объяснить возникновение, а затем распространение по всему миру затейливого рисунка извилистой дорожки...",
			"The main unsolved mystery of the ancient symbol remains its origin. Dozens of hypotheses expressed in this regard have not been able to explain the emergence and then spread throughout the world of an intricate pattern of a winding path...",
			25)
	};

	private void Awake()
	{
		for (var i = 0; i < StoryCount; i++) Stories[i].IsCollected = PlayerPrefs.GetInt($"Story_{i}", 0) != 0;
	}

	public static string GetStoryByIndex(int index, Language language)
	{
		if (index >= StoryCount) return null;
		if (Stories[index].IsCollected)
			return language == Language.English ? Stories[index].StoryOnEnglish : Stories[index].StoryOnRussian;

		return null;
	}

	public static void CollectStory(int index)
	{
		if (index >= StoryCount) return;
		Stories[index].IsCollected = true;
		PlayerPrefs.SetInt($"Story_{index}", 1);
	}
}