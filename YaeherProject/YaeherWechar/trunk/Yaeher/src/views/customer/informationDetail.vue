<template>
    <div class="information-detail padding-top">
        <mt-header fixed :title="consultantName">
            <a @click="$router.push({ path: '/information-list'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
			<mt-button v-if="acceptState != '1'"  slot="right">
                <a @click="stickInfo('1')" class="right-white">置顶</a>
            </mt-button> 
			<mt-button v-if="acceptState === '1'"  slot="right">
                <a @click="stickInfo('2')" class="right-white">取消置顶</a>
            </mt-button> 
        </mt-header>
        <div ref="customerBox" class="content contentDis">
			<div @touchstart="moveContent($event)" class="inforCase">
				<ul class="weChatList"> 
					<li  v-for="(item, index) in infoList" :key="index" class="listLeft"> 
						<div class="weTime">
							<div>{{item.createdOn}}</div>
						</div>
						<div v-if="item.messageFrom === 'Consultant'" class="weTitle infoLeft">
							<a class="imaAvatar">
								<!-- <img :src="item.userImage" /> -->
							</a>
							<div v-html="item.content" class="weChatText">
							</div>
						</div>
						<div v-if="item.messageFrom === 'Customer'" class="weTitle infoRight">
							<a class="imaAvatar" >
								<!-- <img :src="item.userImage" /> -->
							</a>
							<div v-html="item.content" class="weChatText">
							</div>
						</div>
					</li>
				</ul>
			</div>
			<div ref="heightAdd" class="heightAdd"></div>
        </div>
		<div :class="{focusAbsoult: focusFlag}" ref="sendOut" class="sendOut">
			<div class="inputChat" ref="inputChat">
				<textarea @focus="iptFocus($event)"  @blur="iptBlur"  class=""   placeholder=""  rows="1"  ref="address" v-model="content"></textarea>
			</div>
			<div @click="consultationReply()" class="sendChat">发送</div>
		</div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
let moment = require('moment');
let interval
let focusTime
export default {
    data () {
        return {
			id: 0,
			infoList: [],
			consultantName: '',
			content: '',
			fromUserName: '',
			acceptState: 2,
			inFocus: false,
			focusFlag: false,
			sendFlag: true
        }
    },
    methods: {
		iptFocus(evt) {
			this.inFocus = true
			this.focusFlag = true
			let _this = this
			focusTime = setInterval(function () {
				document.body.scrollTop = document.body.scrollHeight
				_this.$refs.address.scrollIntoView()
			},500);
		},
		iptBlur() {
			let _this = this
			this.focusFlag = false
			clearInterval(focusTime)
			
			_this.inFocus = false
			setTimeout(function () {
				if(_this.inFocus == false){
					_this.checkWxScroll()
				}	
			}, 200)
		},
		getInforList() { // 获取聊天列表
			const id = this.id
			let _this = this
            this.instance.acceptWecharStateId({
				id
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
                        this.infoList = response.data.result.item
                        for (let i = 0; i < this.infoList.length; i++) {
							this.infoList[i].createdOn = moment(this.infoList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
							this.infoList[i].content = this.infoList[i].content.replace(/\n/g,"<br/>")
						}
						this.$nextTick(function(){
							_this.$refs.customerBox.scrollTop = _this.$refs.customerBox.scrollHeight;
						})
                    }
                    
                })
                .catch((error) => {
                }) 
            
		},
		moveContent () {
			let _this = this
			_this.$refs.address.blur()
		},
		consultationReply () { // 回复
			let _this = this
			if (!this.sendFlag) {
				Toast('不可重复提交')
				return
			}
			this.sendFlag = false
			this.$indicator.open({
                text: '发送中，请稍候',
                spinnerType: 'fading-circle'
            })
			const id = this.id
			const fromUserName = this.fromUserName
			const content = this.content
			if (!content) {
				Toast('回复内容不能为空')
				return
			}
			this.instance.sendWechaMessgae({
				fromUserName,
				content,
				msgType: 'text'
            })
                .then((response) => {
					this.$indicator.close()
					this.sendFlag = true
                    if (response.data.result.code === 200) {
						this.content= ''
						this.$refs.address.blur()
						this.getInforList()
						

                    }
                    
                })
                .catch((error) => {
					Toast('发送失败')
					this.sendFlag = true
					this.$indicator.close()
                }) 
		},
		stickInfo(index) { // 置顶
			const id = this.id
			let acceptState = index
			this.instance.updateWecharState({
				id,
				acceptState
            })
                .then((response) => {
                    if (response.data.result.code === 200) {
						Toast('置顶成功')
						this.$router.push({ 
							path: '/information-list'
						})
                    }
                    
                })
                .catch((error) => {
                }) 
		},
		// returnGo () {
		// 	this.$router.go(-1)
		// },
		getHeight (el) {
			this.timer = setTimeout(() => {
				el.style.height = 'auto' // 必须设置为auto
				if(el.scrollHeight <= 128) {
					el.style.height = (el.scrollHeight) + 'px'
				} else {
					el.style.height = 128 + 'px'
				}
			}, 20)
		},
		getHeight1 (el, el1) {
			this.timer1 = setTimeout(() => {
				el.style.height = 'auto' // 必须设置为auto
				if(el1.scrollHeight <= 128) {
					el.style.height = (el1.scrollHeight) + 'px'
				} else {
					el.style.height = 128 + 'px'
				}
			}, 20)
		},
		checkWxScroll(){
			var ua = navigator.userAgent.toLowerCase()
			var u = navigator.userAgent.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/)
			if(ua.match(/MicroMessenger/i) == 'micromessenger'&&!!u){
				this.temporaryRepair()
			}
		},
		temporaryRepair(){
			var currentPosition,timer
			var speed = 1
			timer=setInterval(function(){
				currentPosition=document.documentElement.scrollTop || document.body.scrollTop
				currentPosition-=speed
				window.scrollTo(0,0)
				clearInterval(timer)
			}, 1)
		},
		
	},	
	watch: {
		'content': function (newVal) {
			this.getHeight(this.$refs.address)
			this.getHeight1(this.$refs.sendOut, this.$refs.address)
			this.getHeight1(this.$refs.heightAdd, this.$refs.address)
			this.getHeight1(this.$refs.inputChat, this.$refs.address)
		}
	},
	beforeDestroy() {
		clearInterval(interval);
	},
    mounted () {
		this.getInforList()
		let _this = this
		interval = window.setInterval(function() {
				_this.getInforList()
            }, 10000)
	},
	created () {
		this.id = parseInt(this.$route.query.id)
		this.consultantName = this.$route.query.consultantName
		this.fromUserName = this.$route.query.fromUserName
		this.acceptState= this.$route.query.acceptState
		console.log(this.acceptState)
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.information-detail {
	-webkit-user-select: text;
    -moz-user-select: text;
    -ms-user-select: text;
	user-select: text;
	.heightAdd{width: 100%; height: auto;}
	// position: relative;
	.contentDis{
		position: absolute;
		top: 42px;
		left: 0;
		right: 0;
		bottom: 45px;
		overflow: auto;
    	-webkit-overflow-scrolling: touch;
		.inforCase {
			
			.weChatList {
				padding: 0;
				width: 100%;
				li {
					overflow: hidden;
					border: none;
					.weTime {
						width: 100%;
						display: flex; 
						justify-content: center; 
						align-items: center; 
						padding-bottom: 10px;
						div {
							padding: 4px 6px; 
							background: #aaa; 
							color: $color-wfont; 
							font-size: $font-m; 
							border-radius: 3px;
						}
					}
					.weTitle {
						position: relative;
						.weChatText {
							display: inline-block;
							background: $default-color;
							padding: 10px;
							border-radius: 3px;
							color: #fff;
							font-size: $font-l;
							max-width: 260px;
							line-height: 18px;
							word-wrap:break-word
						}
						.imaAvatar {
							position: absolute;
							top: 0;
							display: block;
							img {
								width: 35px;
								height: 35px;
								border-radius: 3px;
							}
						}
					}
					.infoLeft {
						padding-left: 20px;
						float: left;
						.imaAvatar {
							left: 10px;
						}
					}
					.infoRight {
						padding-right: 20px;
						float: right;
						.imaAvatar {
							right: 10px;
						}
					}
				}
			}
		}
	}
	.sendOut {height: 18px; border-top: 1px solid #999; position: fixed; text-align: center; padding: 13px 0;background: #ccc; bottom: 0%; left: 0%; right: 0%;  z-index: 1000;}
	.sendOut > div { font-size: $font-l; line-height: 31px; padding: 0 10px;}
	.sendOut .sendChat {background-color: $color-green; height: 30px; padding: 0 8px; color: #fff;border-radius: 3px; position: absolute; right: 10px; bottom: 8px; z-index: 2000;}
	.sendOut .inputChat { height: 18px; font-size: 16px; background: #fff; border-radius: 3px; padding: 4.5px 0px 6.5px; line-height: 17px; position: absolute; left: 15px; right: 70px; bottom: 8px; z-index: 2;}
	.inputChat textarea {border: none; font-size: 16px; outline: none; padding: 0 10px;resize: none; width: 100%; box-sizing: border-box; display: inline; position: absolute; bottom: 5px; left: 0px; right: 0px; }
	.focusAbsoult {
		position: absolute;
	}
}


</style>