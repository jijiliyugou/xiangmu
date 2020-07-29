<template>
    <div class="guide padding-top">
		<mt-header fixed title="指南">
        </mt-header>
        <div class="p-content listAbout">
            <ul v-infinite-scroll="getArticleList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="mui-table-view guide-margin">
				<li v-for="(item, index) in articleList" :key="index" class="mui-table-view-cell">
			        <router-link :to="{path: '/guide-detail', query: {id: item.id}}" class="mui-navigate-right articleList">
			        	<div style="width: 60px; height: 60px; padding-left: 0;">
							<img v-if="item.imageFie != null" :src="item.imageFie" align="avater" />
                            <img v-if="item.imageFie === null" src="../../assets/image/avater1.jpg" alt="" />
						</div>
						<div class="articleCase">
			        		<p class="blod">{{item.rulesTitle}}</p>
			        		<!-- <p class="line-text">{{item.rulesContent}}</p>
							<p>{{item.createdOn}}</p> -->
							<i class="mint-cell-allow-right"></i>
			        	</div>
			        </router-link>
			    </li>
			</ul>
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-doctor">
                <img slot="icon" src="../../assets/image/question-icon.png">
                咨询
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/guide">
                <img slot="icon" src="../../assets/image/class-icon.png">
                指南
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/doctor-user">
                <img slot="icon" src="../../assets/image/user-icon.png">
                我的
            </mt-tab-item>
        </mt-tabbar>
    </div>
</template>

<script>
import { Tabbar, TabItem, Toast } from 'mint-ui';
import { createSecret } from 'assets/js/common.js'
let moment = require('moment');
export default {
    data () {
        return {
			selected: 'nav2',
			articleList: [],
			skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
			loading: true,
			homeTop: 0
        }
	},
	methods: {
		getArticleList () {
			this.loading = true
			const maxResultCount  = this.maxResultCount 
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.doctorRulesPage({
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
						if (response.data.result.item.items) {
							this.articleList = this.articleList.concat(response.data.result.item.items)
							for (let i = 0; i < this.articleList.length; i++) {
								this.articleList[i].rulesContent = this.articleList[i].rulesContent.replace(/<[^>]+>|&/g,"").replace(/nbsp;/g,"")
								this.articleList[i].createdOn = moment(this.articleList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
							}
							this.skipCount ++
							this.totalPage = response.data.result.item.totalPage
							console.log(this.skipCount, this.totalPage)
						}
					}, 100);
					
                })
                .catch((error) => {
					this.loading = true
                }) 
		}
	},
	mounted () {
		this.getArticleList()
	},
	created () {
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.guide {
	.listAbout {position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;}
	.guide-margin {margin: 10px 0px 55px}
	.articleList {overflow: hidden; display: flex; position: relative;}
	.articleList img {float: left; width: 60px; height: 60px; border-radius: 8px;}
	.articleCase {flex: 1; padding-left: 10px;}
	.articleCase p {color: #333; font-size: $font-m;}
	.articleCase .blod {font-size: $font-l; line-height: 61px;}
	.articleCase .line-text {width: 230px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;}
}

</style>