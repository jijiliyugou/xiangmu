<template>
    <div class="look-question">
        <div ref="srcollBox" class="content listAbout">
			<!-- <div class="searchCase">
				<div class="searchBox">
					<input @keypress="searchKeypress" type="search" v-model="keyWord" placeholder="请输入问答标题" />
					<router-link tag="div" :to="{path: '/question-search', query: {keyWord}}" >搜索</router-link>
				</div>
			</div> -->
			<div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入关键字查询" />
                    <div @click="getQuestionList(true)">搜索</div>
                </div>
            </div>
            <div class="doctorDetailList">
				<ul v-infinite-scroll="getQuestionList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10"  class="mui-table-view">
                    <li v-for="(item, index) in questionList" :key="index" class="mui-table-view-cell lookP">
						<router-link :to="{path: '/question-detail', query: {id: item.id}}" class="mui-navigate-right">
							<!-- <span class="lookHint">{{item.descriptionTiltle}}</span> -->
							<p class="border-dashed">
								<span class="wen-ti">问题：</span>{{item.descriptionTiltle}}
							</p>
							<p class="line-text">
								<span class="hui-da">回答：</span>{{item.answer}}
							</p>
							<!-- <p>
								<span>{{item.doctorName}}</span> <span>{{item.doctorTitle}}</span> <span>{{item.hospital}}</span>
							</p> -->
							<p>
								阅读：{{item.readTotal}}&nbsp;&nbsp; &nbsp;&nbsp;点赞: {{item.upvoteTotal}}
							</p>
						</router-link>
					</li>
				</ul>
			</div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
			keyWord: '',
			questionList: [],
			skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
			loading: true,
			homeTop: 0
        }
	},
    methods: {
		// searchKeypress (event) {
        //     if (event.keyCode == 13) {
        //         this.$router.push({ 
        //             path: '/question-search',
        //             query: {
        //                 keyWord: this.keyWord,
        //             }
        //         })
        //     } 
		// },
		searchKeypress() {
            if (event.keyCode == 13) {
                this.getQuestionList(true)
            } 
        },
		getQuestionList(flag) { // 获取问答列表
			this.loading = true
			if (flag) {
                this.skipCount = 1
                this.totalPage = 2
			}
			const keyWord = this.keyWord
            const maxResultCount  = this.maxResultCount 
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.questionReleasePage({
				keyWord,
				skipCount,
				maxResultCount
            })
                .then((response) => {
					setTimeout(() => {
						this.loading = false
						let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
						if (flag) this.questionList = []
						let questionList1 = response.data.result.item.items
						this.questionList = this.questionList.concat(questionList1)
						console.log(this.questionList)
						for (let i = 0; i < this.questionList.length; i++) {
							this.questionList[i].answer = this.questionList[i].answer.replace(/<[^>]+>/g,"")
						}
						this.skipCount ++
						this.totalPage = response.data.result.item.totalPage
						console.log(this.skipCount, this.totalPage)
					}, 100);	
                })
                .catch((error) => {
					this.loading = true
                }) 
        }
    },
    mounted () {
		this.getQuestionList()
    },
    created () {
	},
	activated () {
        this.$refs.srcollBox.scrollTop = this.homeTop || 0
        
    },
    beforeRouteLeave (to, from, next) {
        let scrollA = this.$refs.srcollBox
        this.homeTop = scrollA.scrollTop || 0
        console.log(this.homeTop )
        next()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.look-question {
	.listAbout {position: absolute; left: 0; right: 0; bottom: 0px;top: 0px;}
	.searchCase {
		padding: 10px;
		// background-color: $default-color; 
		// .searchBox {
		// 	background: $color-wfont url(../../assets/image/search-pic.png) 8px 12px no-repeat; 
		// 	padding: 10px 15px 10px 23px;
		// 	background-size: 14px 14px;
		// 	display: flex;
		// 	line-height: 19px;
		// 	input {
		// 		border: none;
		// 		outline: none;
		// 		flex: 1;
		// 		font-size: $font-l;
		// 	}
		// 	div {
		// 		width: 50px;
		// 		text-align: right;
		// 		font-size: $font-l;
		// 		color: $color-bfont;
		// 		line-height: 20px;
		// 	}
		// }
	}
	.lookP {border-bottom: 1px dashed #ccc;}
	.lookHint {color: $color-wfont; font-size: $font-l; background: $default-color; display: block; padding: 3px 5px 1px; line-height: 16px;  border-radius: 9px; display: inline-block;}
	.lookP p {color: $color-afont; padding: 5px 0;}
	.clickOk {display: flex; justify-content: center; align-items: center; padding: 20px 0;}
	.clickOk div {padding: 5px 10px; border: 1px solid color-bfont; color: color-bfont;}
	.lookP .border-dashed {border-bottom: 1px dashed #eee;}
	.lookP .line-text {width: 250px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;}
	.wen-ti {color: $color-red;}
	.hui-da {color: $color-green;}
}

</style>