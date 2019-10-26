using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace ai.test
{
    public class MessageSerializerTest
    {

        [Fact]
        public void Test_Parsing_Matches_CSharp_Keys_Despite_Snake_Case_And_Lower_Case()
        {
            var updateJSON = @"{
                player: 0,
                turn: 12,
                time: 3,
                'unit_updates': [{id:16}],
                'tile_updates': [{x: 12, y: 15, units: [{id:60}]}],
                }";

            var gameUpate = new MessageSerializer().ParseUpdate(updateJSON);
            gameUpate.Player.Should().Be(0);
            gameUpate.Turn.Should().Be(12);
            gameUpate.Time.Should().Be(3);
            gameUpate.UnitUpdates.First().Id.Should().Be(16);
            gameUpate.TileUpdates.First().X.Should().Be(12);
            gameUpate.TileUpdates.First().Y.Should().Be(15);
            gameUpate.TileUpdates.First().Units.First().Id.Should().Be(60);
        }

        [Fact]
        public void Test_Game_Info_Parsed_Correctly()
        {
            var updateJSON = @"{
                'game_info': {
                    'map_width': 32,
                    'map_height': 36,
                    'game_duration': 30,
                    'turn_duration': 200,
                    'unit_info': {
                    'base': {
                        'hp': 300,
                        'range': 2
                    },
                    'worker': {
                        'cost': 100,
                        'hp': 10,
                        'range': 2,
                        'speed': 5,
                        'attack_damage': 2,
                        'attack_type': 'melee',
                        'attack_cooldown_duration': 30,
                        'can_carry': true,
                        'create_time': 50
                    },
                    'scout': {
                        'cost': 130,
                        'hp': 5,
                        'range': 5,
                        'speed': 3,
                        'attack_damage': 1,
                        'attack_type': 'melee',
                        'attack_cooldown_duration': 30,
                        'create_time': 100
                    },
                    'tank': {
                        'cost': 150,
                        'hp': 30,
                        'range': 2,
                        'speed': 10,
                        'attack_damage': 4,
                        'attack_type': 'ranged',
                        'attack_cooldown_duration': 70,
                        'create_time': 150
                    }
                    }
                },
                'player': 0,
                'turn': 0,
                'time': 299820
                }";
            var gameUpate = new MessageSerializer().ParseUpdate(updateJSON);
            gameUpate.GameInfo.Should().NotBeNull();
            gameUpate.GameInfo.MapWidth.Should().Be(32);
            gameUpate.GameInfo.MapHeight.Should().Be(36);
            gameUpate.GameInfo.GameDuration.Should().Be(30);
            gameUpate.GameInfo.TurnDuration.Should().Be(200);
            gameUpate.GameInfo.UnitInfo["base"].HP.Should().Be(300);
            gameUpate.GameInfo.UnitInfo["base"].Range.Should().Be(2);
            gameUpate.GameInfo.UnitInfo["worker"].Cost.Should().Be(100);
            gameUpate.GameInfo.UnitInfo["worker"].HP.Should().Be(10);
            gameUpate.GameInfo.UnitInfo["worker"].Range.Should().Be(2);
            gameUpate.GameInfo.UnitInfo["worker"].Speed.Should().Be(5);
            gameUpate.GameInfo.UnitInfo["worker"].AttackDamage.Should().Be(2);
            gameUpate.GameInfo.UnitInfo["worker"].AttackType.Should().Be("melee");
            gameUpate.GameInfo.UnitInfo["worker"].AttackCooldownDuration.Should().Be(30);
            gameUpate.GameInfo.UnitInfo["worker"].CanCarry.Should().BeTrue();
            gameUpate.GameInfo.UnitInfo["worker"].CreateTime.Should().Be(50);


            gameUpate.GameInfo.UnitInfo["tank"].Cost.Should().Be(150);
            gameUpate.GameInfo.UnitInfo["tank"].HP.Should().Be(30);
            gameUpate.GameInfo.UnitInfo["tank"].Range.Should().Be(2);
            gameUpate.GameInfo.UnitInfo["tank"].Speed.Should().Be(10);
            gameUpate.GameInfo.UnitInfo["tank"].AttackDamage.Should().Be(4);
            gameUpate.GameInfo.UnitInfo["tank"].AttackType.Should().Be("ranged");
            gameUpate.GameInfo.UnitInfo["tank"].AttackCooldownDuration.Should().Be(70);
            gameUpate.GameInfo.UnitInfo["tank"].CanCarry.Should().BeFalse();
            gameUpate.GameInfo.UnitInfo["tank"].CreateTime.Should().Be(150);
        }


        [Fact]
        public void Test_Commands_Serialized_Correctly()
        {
            IEnumerable<AICommand> commands = new List<AICommand>() {
                new AICommand() { Command = "MOVE", Unit = 2, Dir = "N", Type = "scout", Dx = 1, Dy = -1, Target = 3 }
            };
            var message = new AICommandsMessage() { Commands = commands };
            var serialized = new MessageSerializer().SerializeAICommandsMessage(message);
            var expected = "{\"commands\":[{\"command\":\"MOVE\",\"unit\":2,\"dir\":\"N\",\"type\":\"scout\",\"dx\":1,\"dy\":-1,\"target\":3}]}\n";
            serialized.Should().Be(expected);
        }
    }
}