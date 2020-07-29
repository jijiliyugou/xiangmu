<template>
    <div class="question-search padding-top">
        <mt-header fixed title="搜索结果">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
			<div class="searchCase">
				<div class="searchBox">
					<input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入问答标题" />
					<div @click="getQuestionList">搜索</div>
				</div>
			</div>
            <div class="doctorDetailList">
				<ul class="mui-table-view">
                    <li v-for="(item, index) in questionList" :key="index" class="mui-table-view-cell lookP">
						<router-link :to="{path: '/question-detail', query: {id: item.id}}" class="mui-navigate-right">
							<span class="lookHint">{{item.title}}</span>
							<p class="border-dashed">
								<span class="wen-ti">问题：</span>{{item.descriptionTiltle}}
							</p>
							<p>
								<span class="hui-da">回答：</span>{{item.answer}}
							</p>
							<p>
								<span>{{item.doctorName}}</span> <span>{{item.doctorTitle}}</span> <span>{{item.hospital}}</span>
							</p>
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
import { createSecret } from 'assets/js/common.js'
export default {
    data () {
        return {
			keyWord: '',
			questionList: []
        }
	},
    methods: {
		searchKeypress (event) {
            if (event.keyCode == 13) {
                this.getQuestionList()
            } 
        },
        getQuestionList() { // 搜索问答
			const keyWord = this.keyWord
			this.$refs.inputText.blur()
            if (!keyWord) {
                Toast('请输入搜索内容');
                return
            }
            this.instance.questionReleasePage({
                keyWord
            })
                .then((response) => {
					this.questionList = response.data.result.item.items
					console.log(this.questionList)
                })
                .catch((error) => {
                    console.log('专业分组请求失败')
                }) 
        }
    },
    mounted () {
        // this.getQuestionList()
    },
    created () {
        this.keyWord = this.$route.query.keyWord
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.question-search {
	// .searchCase {
	// 	padding: 0 10px 10px;
	// 	background-color: $default-color; 
	// 	.searchBox {
	// 		background: $color-wfont url(../../assets/image/search-pic.png) 8px 12px no-repeat; 
	// 		padding: 10px 15px 10px 23px;
	// 		background-size: 14px 14px;
	// 		display: flex;
	// 		line-height: 19px;
	// 		input {
	// 			border: none;
	// 			outline: none;
	// 			flex: 1;
	// 			font-size: $font-l;
	// 		}
	// 		div {
	// 			width: 50px;
	// 			text-align: right;
	// 			font-size: $font-l;
	// 			color: $color-bfont;
	// 			line-height: 20px;
	// 		}
	// 	}
	// }
	.lookHint {color: $color-wfont; font-size: $font-l; background: $default-color; display: block; padding: 3px 5px 1px; line-height: 16px;  border-radius: 9px; display: inline-block;}
	.lookP p {color: $color-afont; padding-top: 10px;}
	.clickOk {display: flex; justify-content: center; align-items: center; padding: 20px 0;}
	.clickOk div {padding: 5px 10px; border: 1px solid color-bfont; color: color-bfont;}
	.lookP .border-dashed {border-bottom: 1px dashed #ccc;}
	.wen-ti {color: $color-red;}
	.hui-da {color: $color-green;}
}

</style>